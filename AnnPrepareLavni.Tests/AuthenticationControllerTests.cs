using System.Security.Claims;
using AnnPrepareLavni.ApiService.Data;
using AnnPrepareLavni.ApiService.Features.Authentication;
using AnnPrepareLavni.ApiService.Features.Authentication.Contracts;
using AnnPrepareLavni.ApiService.Features.User;
using AnnPrepareLavni.ApiService.Models;
using AnnPrepareLavni.ApiService.Models.Enums;
using AnnPrepareLavni.ApiService.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnnPrepareLavni.Tests;

[TestClass]
public class AuthenticationControllerTests
{
    private const string PhoneUserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 15_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.0 Mobile/15E148 Safari/604.1";
    private const string ComputerUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36";
 
    private ApplicationDbContext _context = null!;
    private AuthenticationController _controller = null!;
    private IUserService _userService = null!;
    private IAuthenticationService _authenticationService = null!;
    private IConfiguration _configuration = null!;

    [TestInitialize]
    public async Task TestInitialize()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestAuthDb_" + Guid.NewGuid())
            .Options;

        var inMemorySettings = new Dictionary<string, string?> {
            {"JwtSettings:SecretKey", "ThisIsASecretKeyForJwtTokenWhichIsLongEnough"},
            {"JwtSettings:AccessTokenExpirationInMinutes", "15"},
            {"JwtSettings:RefreshTokenExpirationInMinutes", "1440"},
            {"JwtSettings:Issuer", "TestIssuer"},
            {"JwtSettings:Audience", "TestAudience"}
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        var authLogger = loggerFactory?.CreateLogger<AuthenticationService>();
        var userLogger = loggerFactory?.CreateLogger<UserService>();

        _context = new ApplicationDbContext(options);

        _authenticationService = new AuthenticationService(_context, authLogger!, _configuration);
        _userService = new UserService(_context, userLogger!);
        _controller = new AuthenticationController(_userService, _authenticationService);

        await SeedTestUsers();
    }

    private async Task SeedTestUsers()
    {
        var testUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            PasswordHash = "password",
            FirstName = "Test",
            LastName = "User",
            Email = "testuser@example.com",
            Role = UserRole.Receptionist,
            Language = Language.English
        };

        await _userService.CreateAsync(testUser);
    }

    [TestMethod]
    public async Task Login_WithValidCredentials_ShouldReturnAccessAndRefreshToken()
    {
        var request = new AuthenticationRequest
        {
            Username = "testuser",
            Password = "password"
        };

        var result = await _controller.Login(request);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult, "Expected OkObjectResult.");
        Assert.AreEqual(200, okResult.StatusCode, "Expected status code 200.");

        var authResponse = okResult.Value as AuthenticationResponse;
        Assert.IsNotNull(authResponse, "Expected AuthenticationResponse.");
        Assert.IsFalse(string.IsNullOrEmpty(authResponse.AccessToken), "AccessToken should not be null or empty.");
        Assert.IsFalse(string.IsNullOrEmpty(authResponse.RefreshToken), "RefreshToken should not be null or empty.");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
        Assert.IsNotNull(user, "Test user should exist.");

        var hashedRefreshToken = _authenticationService.HashToken(authResponse.RefreshToken);
        var savedToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedRefreshToken);
        Assert.IsNotNull(savedToken, "Refresh token should be saved in the database.");
    }

    [TestMethod]
    public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
    {
        var request = new AuthenticationRequest
        {
            Username = "testuser",
            Password = "wrongpassword"
        };

        var result = await _controller.Login(request);

        var unauthorizedResult = result as UnauthorizedObjectResult;
        Assert.IsNotNull(unauthorizedResult, "Expected UnauthorizedObjectResult.");
        Assert.AreEqual(401, unauthorizedResult.StatusCode, "Expected status code 401.");

        var response = unauthorizedResult.Value as ReturnMessage;
        Assert.IsNotNull(response, "Response should not be null.");
        Assert.AreEqual("Username or password is incorrect", response.Message, "Unexpected error message.");
    }

    [TestMethod]
    public async Task Refresh_WithValidTokens_ShouldReturnNewTokens()
    {
        var loginRequest = new AuthenticationRequest
        {
            Username = "testuser",
            Password = "password"
        };

        var loginResult = await _controller.Login(loginRequest);
        var okLoginResult = loginResult as OkObjectResult;
        Assert.IsNotNull(okLoginResult, "Expected OkObjectResult from login.");
        var authResponse = okLoginResult?.Value as AuthenticationResponse;
        Assert.IsNotNull(authResponse, "AuthenticationResponse should not be null.");

        var oldAccessToken = authResponse!.AccessToken;
        var oldRefreshToken = authResponse.RefreshToken;

        var refreshRequest = new RefreshTokenRequest
        {
            AccessToken = oldAccessToken,
            RefreshToken = oldRefreshToken
        };

        var refreshResult = await _controller.Refresh(refreshRequest);

        var okRefreshResult = refreshResult as OkObjectResult;
        Assert.IsNotNull(okRefreshResult, "Expected OkObjectResult from refresh.");
        Assert.AreEqual(200, okRefreshResult.StatusCode, "Expected status code 200.");

        var newAuthResponse = okRefreshResult.Value as AuthenticationResponse;
        Assert.IsNotNull(newAuthResponse, "New AuthenticationResponse should not be null.");
        Assert.IsFalse(string.IsNullOrEmpty(newAuthResponse.AccessToken), "New AccessToken should not be null or empty.");
        Assert.IsFalse(string.IsNullOrEmpty(newAuthResponse.RefreshToken), "New RefreshToken should not be null or empty.");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
        Assert.IsNotNull(user, "Test user should exist.");

        var hashedOldRefreshToken = _authenticationService.HashToken(oldRefreshToken);
        var oldToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedOldRefreshToken);
        Assert.IsNull(oldToken, "Old refresh token should be invalidated.");

        var hashedNewRefreshToken = _authenticationService.HashToken(newAuthResponse.RefreshToken);
        var newToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedNewRefreshToken);
        Assert.IsNotNull(newToken, "New refresh token should be saved in the database.");
    }

    [TestMethod]
    public async Task Refresh_WithInvalidAccessToken_ShouldReturnUnauthorized()
    {
        var refreshRequest = new RefreshTokenRequest
        {
            AccessToken = "invalidAccessToken",
            RefreshToken = "someRefreshToken"
        };

        var refreshResult = await _controller.Refresh(refreshRequest);

        var unauthorizedResult = refreshResult as UnauthorizedObjectResult;
        Assert.IsNotNull(unauthorizedResult, "Expected UnauthorizedObjectResult.");
        Assert.AreEqual(401, unauthorizedResult.StatusCode, "Expected status code 401.");

        var response = unauthorizedResult.Value as ReturnMessage;
        Assert.IsNotNull(response, "Response should not be null.");
        Assert.AreEqual("Invalid access token", response.Message, "Unexpected error message.");
    }

    [TestMethod]
    public async Task Refresh_WithInvalidRefreshToken_ShouldReturnUnauthorized()
    {
        var loginRequest = new AuthenticationRequest
        {
            Username = "testuser",
            Password = "password"
        };

        var loginResult = await _controller.Login(loginRequest);
        var okLoginResult = loginResult as OkObjectResult;
        Assert.IsNotNull(okLoginResult, "Expected OkObjectResult from login.");
        var authResponse = okLoginResult?.Value as AuthenticationResponse;
        Assert.IsNotNull(authResponse, "AuthenticationResponse should not be null.");

        var refreshRequest = new RefreshTokenRequest
        {
            AccessToken = authResponse.AccessToken,
            RefreshToken = "invalidRefreshToken"
        };

        var refreshResult = await _controller.Refresh(refreshRequest);

        var unauthorizedResult = refreshResult as UnauthorizedObjectResult;
        Assert.IsNotNull(unauthorizedResult, "Expected UnauthorizedObjectResult.");
        Assert.AreEqual(401, unauthorizedResult.StatusCode, "Expected status code 401.");

        var response = unauthorizedResult.Value as ReturnMessage;
        Assert.IsNotNull(response, "Response should not be null.");
        Assert.AreEqual("Invalid or expired refresh token", response.Message, "Unexpected error message.");
    }

    [TestMethod]
    public async Task Logout_WithValidRefreshToken_ShouldRemoveThatRefreshToken()
    {
        var loginRequest = new AuthenticationRequest
        {
            Username = "testuser",
            Password = "password"
        };

        var loginResult = await _controller.Login(loginRequest);
        var okLoginResult = loginResult as OkObjectResult;
        Assert.IsNotNull(okLoginResult, "Expected OkObjectResult from login.");
        var authResponse = okLoginResult?.Value as AuthenticationResponse;
        Assert.IsNotNull(authResponse, "AuthenticationResponse should not be null.");

        var accessToken = authResponse!.AccessToken;
        var refreshToken = authResponse.RefreshToken;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
        Assert.IsNotNull(user, "Test user should exist.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.Role, user.Role.ToString()),
            new(ClaimTypes.Email, user.Email)
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        var logoutResult = await _controller.Logout(refreshToken);

        var noContentResult = logoutResult as NoContentResult;
        Assert.IsNotNull(noContentResult, "Expected NoContentResult.");
        Assert.AreEqual(204, noContentResult.StatusCode, "Expected status code 204.");

        var hashedRefreshToken = _authenticationService.HashToken(refreshToken);
        var savedToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedRefreshToken);
        Assert.IsNull(savedToken, "Refresh token should be removed from the database.");
    }

    [TestMethod]
    public async Task Logout_WithoutRefreshToken_ShouldRemoveAllRefreshTokens()
    {
        var loginRequest1 = new AuthenticationRequest
        {
            Username = "testuser",
            Password = "password"
        };

        var loginResult1 = await _controller.Login(loginRequest1);
        var okLoginResult1 = loginResult1 as OkObjectResult;
        Assert.IsNotNull(okLoginResult1, "Expected OkObjectResult from first login.");
        var authResponse1 = okLoginResult1?.Value as AuthenticationResponse;
        Assert.IsNotNull(authResponse1, "AuthenticationResponse from first login should not be null.");

        var accessToken1 = authResponse1!.AccessToken;
        var refreshToken1 = authResponse1.RefreshToken;

        var loginRequest2 = new AuthenticationRequest
        {
            Username = "testuser",
            Password = "password"
        };

        var loginResult2 = await _controller.Login(loginRequest2);
        var okLoginResult2 = loginResult2 as OkObjectResult;
        Assert.IsNotNull(okLoginResult2, "Expected OkObjectResult from second login.");
        var authResponse2 = okLoginResult2?.Value as AuthenticationResponse;
        Assert.IsNotNull(authResponse2, "AuthenticationResponse from second login should not be null.");

        var accessToken2 = authResponse2!.AccessToken;
        var refreshToken2 = authResponse2.RefreshToken;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
        Assert.IsNotNull(user, "Test user should exist.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.Role, user.Role.ToString()),
            new(ClaimTypes.Email, user.Email)
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        var logoutResult = await _controller.Logout(null!);

        var noContentResult = logoutResult as NoContentResult;
        Assert.IsNotNull(noContentResult, "Expected NoContentResult.");
        Assert.AreEqual(204, noContentResult.StatusCode, "Expected status code 204.");

        var savedTokens = await _context.RefreshTokens.Where(rt => rt.UserId == user!.Id).ToListAsync();
        Assert.AreEqual(0, savedTokens.Count, "All refresh tokens should be removed from the database.");
    }
    [TestMethod]
    public async Task Login_FromComputerAndPhone_ShouldStoreSeparateRefreshTokens()
    {
        // Arrange
        var computerUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36";
        var phoneUserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 15_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.0 Mobile/15E148 Safari/604.1";

        // Act
        // Login from Computer
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                Request =
                    {
                        Headers =
                        {
                            { "User-Agent", computerUserAgent }
                        }
                    }
            }
        };
        var loginRequestComputer = new AuthenticationRequest
        {
            Username = "testuser",
            Password = "password"
        };
        var loginResultComputer = await _controller.Login(loginRequestComputer);
        var okLoginComputer = loginResultComputer as OkObjectResult;
        Assert.IsNotNull(okLoginComputer, "Expected OkObjectResult from computer login.");
        var authResponseComputer = okLoginComputer?.Value as AuthenticationResponse;
        Assert.IsNotNull(authResponseComputer, "AuthenticationResponse from computer login should not be null.");

        // Login from Phone
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                Request =
                    {
                        Headers =
                        {
                            { "User-Agent", phoneUserAgent }
                        }
                    }
            }
        };
        var loginRequestPhone = new AuthenticationRequest
        {
            Username = "testuser",
            Password = "password"
        };
        var loginResultPhone = await _controller.Login(loginRequestPhone);
        var okLoginPhone = loginResultPhone as OkObjectResult;
        Assert.IsNotNull(okLoginPhone, "Expected OkObjectResult from phone login.");
        var authResponsePhone = okLoginPhone?.Value as AuthenticationResponse;
        Assert.IsNotNull(authResponsePhone, "AuthenticationResponse from phone login should not be null.");

        // Assert
        // Verify that two separate refresh tokens are stored for the user with different device info
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
        Assert.IsNotNull(user, "Test user should exist.");

        var hashedRefreshTokenComputer = _authenticationService.HashToken(authResponseComputer.RefreshToken);
        var savedTokenComputer = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedRefreshTokenComputer && rt.DeviceInfo == computerUserAgent);
        Assert.IsNotNull(savedTokenComputer, "Refresh token for computer should be saved in the database.");

        var hashedRefreshTokenPhone = _authenticationService.HashToken(authResponsePhone.RefreshToken);
        var savedTokenPhone = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedRefreshTokenPhone && rt.DeviceInfo == phoneUserAgent);
        Assert.IsNotNull(savedTokenPhone, "Refresh token for phone should be saved in the database.");

        // Ensure that tokens are distinct
        Assert.AreNotEqual(authResponseComputer.RefreshToken, authResponsePhone.RefreshToken, "Refresh tokens for different devices should be distinct.");
    }

    [TestMethod]
    public async Task Refresh_WithExpiredRefreshToken_ShouldReturnUnauthorized()
    {
        // Arrange
        // Perform a successful login to obtain tokens
        var loginRequest = new AuthenticationRequest
        {
            Username = "testuser",
            Password = "password"
        };

        var loginResult = await _controller.Login(loginRequest);
        var okLoginResult = loginResult as OkObjectResult;
        Assert.IsNotNull(okLoginResult, "Expected OkObjectResult from login.");
        var authResponse = okLoginResult?.Value as AuthenticationResponse;
        Assert.IsNotNull(authResponse, "AuthenticationResponse should not be null.");

        var accessToken = authResponse!.AccessToken;
        var refreshToken = authResponse.RefreshToken;

        // Simulate token expiration by setting ExpiryDateUtc to a past date
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
        Assert.IsNotNull(user, "Test user should exist.");

        var hashedRefreshToken = _authenticationService.HashToken(refreshToken);
        var savedToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedRefreshToken);
        Assert.IsNotNull(savedToken, "Refresh token should exist.");

        savedToken.ExpiryDateUtc = DateTime.UtcNow.AddMinutes(-1); // Expire the token
        _context.RefreshTokens.Update(savedToken);
        await _context.SaveChangesAsync();

        // Create a refresh request with the expired refresh token
        var refreshRequest = new RefreshTokenRequest
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        // Act
        var refreshResult = await _controller.Refresh(refreshRequest);

        // Assert
        var unauthorizedResult = refreshResult as UnauthorizedObjectResult;
        Assert.IsNotNull(unauthorizedResult, "Expected UnauthorizedObjectResult.");
        Assert.AreEqual(401, unauthorizedResult.StatusCode, "Expected status code 401.");

        var response = unauthorizedResult.Value as ReturnMessage;
        Assert.IsNotNull(response, "Response should not be null.");
        Assert.AreEqual("Invalid or expired refresh token", response.Message, "Unexpected error message.");

        // Verify that the expired refresh token is not replaced or stored again
        var expiredToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedRefreshToken);
        Assert.IsNull(expiredToken, "Expired refresh token should be removed when invalidated.");
    }

    [TestMethod]
    public async Task Refresh_WithOneExpiredAndOneValidRefreshToken_ShouldHandleCorrectly()
    {
        var authResponseComputer = await Login_FromComputer();
        Assert.IsNotNull(authResponseComputer, "AuthenticationResponse from computer login should not be null.");

        var accessTokenComputer = authResponseComputer.AccessToken;
        var refreshTokenComputer = authResponseComputer.RefreshToken;

        var authResponsePhone = await Login_FromPhone();
        Assert.IsNotNull(authResponsePhone, "AuthenticationResponse from phone login should not be null.");

        var accessTokenPhone = authResponsePhone.AccessToken;
        var refreshTokenPhone = authResponsePhone.RefreshToken;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
        Assert.IsNotNull(user, "Test user should exist.");

        var hashedRefreshTokenComputer = _authenticationService.HashToken(refreshTokenComputer);
        var savedTokenComputer = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedRefreshTokenComputer && rt.DeviceInfo == ComputerUserAgent);
        Assert.IsNotNull(savedTokenComputer, "Computer refresh token should exist.");

        savedTokenComputer.ExpiryDateUtc = DateTime.UtcNow.AddMinutes(-1); // Expire the computer's refresh token
        _context.RefreshTokens.Update(savedTokenComputer);
        await _context.SaveChangesAsync();

        // Attempt to refresh using the expired computer refresh token
        var refreshRequestComputer = new RefreshTokenRequest
        {
            AccessToken = accessTokenComputer,
            RefreshToken = refreshTokenComputer
        };

        var refreshResultComputer = await _controller.Refresh(refreshRequestComputer);

        // Assert
        var unauthorizedResult = refreshResultComputer as UnauthorizedObjectResult;
        Assert.IsNotNull(unauthorizedResult, "Expected UnauthorizedObjectResult for expired computer refresh token.");
        Assert.AreEqual(401, unauthorizedResult.StatusCode, "Expected status code 401.");

        var response = unauthorizedResult.Value as ReturnMessage;
        Assert.IsNotNull(response, "Response should not be null.");
        Assert.AreEqual("Invalid or expired refresh token", response.Message, "Unexpected error message for computer refresh.");

        // Ensure that the phone's refresh token is still valid
        var refreshRequestPhone = new RefreshTokenRequest
        {
            AccessToken = accessTokenPhone,
            RefreshToken = refreshTokenPhone
        };

        var refreshResultPhone = await _controller.Refresh(refreshRequestPhone);

        var okRefreshPhone = refreshResultPhone as OkObjectResult;
        Assert.IsNotNull(okRefreshPhone, "Expected OkObjectResult from phone refresh.");
        Assert.AreEqual(200, okRefreshPhone.StatusCode, "Expected status code 200 for phone refresh.");

        var newAuthResponsePhone = okRefreshPhone.Value as AuthenticationResponse;
        Assert.IsNotNull(newAuthResponsePhone, "New AuthenticationResponse for phone should not be null.");
        Assert.IsFalse(string.IsNullOrEmpty(newAuthResponsePhone.AccessToken), "New AccessToken for phone should not be null or empty.");
        Assert.IsFalse(string.IsNullOrEmpty(newAuthResponsePhone.RefreshToken), "New RefreshToken for phone should not be null or empty.");

        // Verify that the old phone refresh token is invalidated
        var hashedOldRefreshTokenPhone = _authenticationService.HashToken(refreshTokenPhone);
        var oldPhoneToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedOldRefreshTokenPhone);
        Assert.IsNull(oldPhoneToken, "Old phone refresh token should be invalidated.");

        // Verify that the new phone refresh token is stored
        var hashedNewRefreshTokenPhone = _authenticationService.HashToken(newAuthResponsePhone.RefreshToken);
        var newPhoneToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedNewRefreshTokenPhone);
        Assert.IsNotNull(newPhoneToken, "New phone refresh token should be saved in the database.");
    }

    [TestMethod]
    public async Task AccessToken_Expired_ShouldAllowRefreshWithValidRefreshToken()
    {
        var authResponsePhone = await Login_FromPhone();
        Assert.IsNotNull(authResponsePhone, "AuthenticationResponse from phone login should not be null.");

        var accessTokenPhone = authResponsePhone.AccessToken;
        var refreshTokenPhone = authResponsePhone.RefreshToken;

        var refreshRequest = new RefreshTokenRequest
        {
            AccessToken = accessTokenPhone, // Assume access token is expired
            RefreshToken = refreshTokenPhone
        };

        var refreshResult = await _controller.Refresh(refreshRequest);

        var okRefreshResult = refreshResult as OkObjectResult;
        Assert.IsNotNull(okRefreshResult, "Expected OkObjectResult from refresh.");
        Assert.AreEqual(200, okRefreshResult.StatusCode, "Expected status code 200.");

        var newAuthResponse = okRefreshResult.Value as AuthenticationResponse;
        Assert.IsNotNull(newAuthResponse, "New AuthenticationResponse should not be null.");
        Assert.IsFalse(string.IsNullOrEmpty(newAuthResponse.AccessToken), "New AccessToken should not be null or empty.");
        Assert.IsFalse(string.IsNullOrEmpty(newAuthResponse.RefreshToken), "New RefreshToken should not be null or empty.");
    }

    [TestMethod]
    public async Task Logout_FromOneDevice_ShouldNotAffectOtherDevices()
    {
        var authResponseComputer = await Login_FromComputer();
        Assert.IsNotNull(authResponseComputer, "AuthenticationResponse from computer login should not be null.");

        var accessTokenComputer = authResponseComputer.AccessToken;
        var refreshTokenComputer = authResponseComputer.RefreshToken;

        var authResponsePhone = await Login_FromPhone();
        Assert.IsNotNull(authResponsePhone, "AuthenticationResponse from phone login should not be null.");

        var accessTokenPhone = authResponsePhone.AccessToken;
        var refreshTokenPhone = authResponsePhone.RefreshToken;

        // Simulate an authenticated user (e.g., computer device) by setting the User property
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
        Assert.IsNotNull(user, "Test user should exist.");

        var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
                new(ClaimTypes.Role, user.Role.ToString()),
                new(ClaimTypes.Email, user.Email)
            };
        var identityComputer = new ClaimsIdentity(claims, "TestAuthComputer");
        var principalComputer = new ClaimsPrincipal(identityComputer);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principalComputer }
        };

        // Act
        // Logout from Computer by providing the computer's refresh token
        var logoutResultComputer = await _controller.Logout(refreshTokenComputer);

        // Assert
        var noContentResultComputer = logoutResultComputer as NoContentResult;
        Assert.IsNotNull(noContentResultComputer, "Expected NoContentResult from computer logout.");
        Assert.AreEqual(204, noContentResultComputer.StatusCode, "Expected status code 204 for computer logout.");

        // Verify that the computer's refresh token is removed
        var hashedComputerRefreshToken = _authenticationService.HashToken(refreshTokenComputer);
        var savedComputerToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedComputerRefreshToken);
        Assert.IsNull(savedComputerToken, "Computer refresh token should be removed from the database.");

        // Verify that the phone's refresh token still exists
        var hashedPhoneRefreshToken = _authenticationService.HashToken(refreshTokenPhone);
        var savedPhoneToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedPhoneRefreshToken);
        Assert.IsNotNull(savedPhoneToken, "Phone refresh token should still exist in the database.");
    }
    
    [TestMethod]
    public async Task<AuthenticationResponse?> Login_FromPhone()
    {

        _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        _controller.ControllerContext.HttpContext.Request.Headers.Append("User-Agent", PhoneUserAgent);

        var loginRequestPhone = new AuthenticationRequest
        {
            Username = "testuser",
            Password = "password"
        };
        var loginResultPhone = await _controller.Login(loginRequestPhone);
        var okLoginPhone = loginResultPhone as OkObjectResult;
        Assert.IsNotNull(okLoginPhone, "Expected OkObjectResult from phone login.");

        var authResponsePhone = okLoginPhone?.Value as AuthenticationResponse;
        Assert.IsNotNull(authResponsePhone, "AuthenticationResponse from phone login should not be null.");

        return authResponsePhone;
    }

    [TestMethod]
    public async Task<AuthenticationResponse?> Login_FromComputer()
    {
        _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        _controller.ControllerContext.HttpContext.Request.Headers.Append("User-Agent", ComputerUserAgent);

        var loginRequestComputer = new AuthenticationRequest
        {
            Username = "testuser",
            Password = "password"
        };
        var loginResultComputer = await _controller.Login(loginRequestComputer);
        var okLoginComputer = loginResultComputer as OkObjectResult;
        Assert.IsNotNull(okLoginComputer, "Expected OkObjectResult from computer login.");
        var authResponseComputer = okLoginComputer?.Value as AuthenticationResponse;
        Assert.IsNotNull(authResponseComputer, "AuthenticationResponse from computer login should not be null.");

        return authResponseComputer;
    }

    [TestMethod]
    public async Task Login_FromComputer_ThenPhone_ThenLogoutPhone_ShouldKeepComputerTokenIntact()
    {
        var authResponseComputer = await Login_FromComputer();
        Assert.IsNotNull(authResponseComputer, "AuthenticationResponse from computer login should not be null.");

        var accessTokenComputer = authResponseComputer.AccessToken;
        var refreshTokenComputer = authResponseComputer.RefreshToken;

        var authResponsePhone = await Login_FromPhone();
        Assert.IsNotNull(authResponsePhone, "AuthenticationResponse from phone login should not be null.");

        var accessTokenPhone = authResponsePhone.AccessToken;
        var refreshTokenPhone = authResponsePhone.RefreshToken;

        // Simulate an authenticated user (e.g., phone device) by setting the User property
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
        Assert.IsNotNull(user, "Test user should exist.");

        var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
                new(ClaimTypes.Role, user.Role.ToString()),
                new(ClaimTypes.Email, user.Email)
            };
        var identityPhone = new ClaimsIdentity(claims, "TestAuthPhone");
        var principalPhone = new ClaimsPrincipal(identityPhone);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principalPhone }
        };

        // Act
        // Logout from Phone by providing the phone's refresh token
        var logoutResultPhone = await _controller.Logout(refreshTokenPhone);

        // Assert
        var noContentResultPhone = logoutResultPhone as NoContentResult;
        Assert.IsNotNull(noContentResultPhone, "Expected NoContentResult from phone logout.");
        Assert.AreEqual(204, noContentResultPhone.StatusCode, "Expected status code 204 for phone logout.");

        // Verify that the phone's refresh token is removed
        var hashedPhoneRefreshToken = _authenticationService.HashToken(refreshTokenPhone);
        var savedPhoneToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedPhoneRefreshToken);
        Assert.IsNull(savedPhoneToken, "Phone refresh token should be removed from the database.");

        // Verify that the computer's refresh token still exists
        var hashedComputerRefreshToken = _authenticationService.HashToken(refreshTokenComputer);
        var savedComputerToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedComputerRefreshToken);
        Assert.IsNotNull(savedComputerToken, "Computer refresh token should still exist in the database.");
    }

    [TestMethod]
    public async Task Refresh_WithExpiredRefreshToken_ShouldNotAffectOtherValidRefreshTokens()
    {
        var authResponseComputer = await Login_FromComputer();
        Assert.IsNotNull(authResponseComputer, "AuthenticationResponse from computer login should not be null.");

        var accessTokenComputer = authResponseComputer.AccessToken;
        var refreshTokenComputer = authResponseComputer.RefreshToken;

        var authResponsePhone = await Login_FromPhone();
        Assert.IsNotNull(authResponsePhone, "AuthenticationResponse from phone login should not be null.");

        var accessTokenPhone = authResponsePhone.AccessToken;
        var refreshTokenPhone = authResponsePhone.RefreshToken;

        // Simulate computer refresh token expiration
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
        Assert.IsNotNull(user, "Test user should exist.");

        var hashedRefreshTokenComputer = _authenticationService.HashToken(refreshTokenComputer);
        var savedTokenComputer = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedRefreshTokenComputer && rt.DeviceInfo == ComputerUserAgent);
        Assert.IsNotNull(savedTokenComputer, "Computer refresh token should exist.");

        savedTokenComputer.ExpiryDateUtc = DateTime.UtcNow.AddMinutes(-1); // Expire the computer's refresh token
        _context.RefreshTokens.Update(savedTokenComputer);
        await _context.SaveChangesAsync();

        var refreshExpiredRequestComputer = new RefreshTokenRequest
        {
            AccessToken = accessTokenComputer,
            RefreshToken = refreshTokenComputer
        };

        var refreshResultComputer = await _controller.Refresh(refreshExpiredRequestComputer);

        var unauthorizedResult = refreshResultComputer as UnauthorizedObjectResult;
        Assert.IsNotNull(unauthorizedResult, "Expected UnauthorizedObjectResult for expired computer refresh token.");
        Assert.AreEqual(401, unauthorizedResult.StatusCode, "Expected status code 401.");

        var response = unauthorizedResult.Value as ReturnMessage;
        Assert.IsNotNull(response, "Response should not be null.");
        Assert.AreEqual("Invalid or expired refresh token", response.Message, "Unexpected error message for computer refresh.");

        var refreshValidRequestPhone = new RefreshTokenRequest
        {
            AccessToken = accessTokenPhone,
            RefreshToken = refreshTokenPhone
        };

        var refreshResultPhone = await _controller.Refresh(refreshValidRequestPhone);

        var okRefreshPhone = refreshResultPhone as OkObjectResult;
        Assert.IsNotNull(okRefreshPhone, "Expected OkObjectResult from phone refresh.");
        Assert.AreEqual(200, okRefreshPhone.StatusCode, "Expected status code 200 for phone refresh.");

        var newAuthResponsePhone = okRefreshPhone.Value as AuthenticationResponse;
        Assert.IsNotNull(newAuthResponsePhone, "New AuthenticationResponse for phone should not be null.");
        Assert.IsFalse(string.IsNullOrEmpty(newAuthResponsePhone.AccessToken), "New AccessToken for phone should not be null or empty.");
        Assert.IsFalse(string.IsNullOrEmpty(newAuthResponsePhone.RefreshToken), "New RefreshToken for phone should not be null or empty.");

        var hashedOldRefreshTokenPhone = _authenticationService.HashToken(refreshTokenPhone);
        var oldPhoneToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedOldRefreshTokenPhone);
        Assert.IsNull(oldPhoneToken, "Old phone refresh token should be invalidated.");

        var hashedNewRefreshTokenPhone = _authenticationService.HashToken(newAuthResponsePhone.RefreshToken);
        var newPhoneToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedNewRefreshTokenPhone);
        Assert.IsNotNull(newPhoneToken, "New phone refresh token should be saved in the database.");

        var expiredComputerToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Token == hashedRefreshTokenComputer);
        Assert.IsNull(expiredComputerToken, "Expired computer refresh token should remain invalidated.");
    }
}