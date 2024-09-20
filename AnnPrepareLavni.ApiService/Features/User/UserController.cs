using AnnPrepareLavni.ApiService.Features.User.Contracts;
using AnnPrepareLavni.ApiService.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace AnnPrepareLavni.ApiService.Features.User;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IValidator<UserRequest> _validator;
    private readonly IUserService _userService;

    public UsersController(
        IValidator<UserRequest> validator,
        IUserService userService)
    {
        _validator = validator;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
    {
        var users = await _userService.GetAllAsync();
        var userResponseList = users.Select(UserMapper.ToResponse);
        return Ok(userResponseList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> GetUser(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid user ID.");
        }

        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var userResponse = UserMapper.ToResponse(user);
        return Ok(userResponse);
    }

    [HttpGet("byRole/{role}")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsersByRole(UserRole role)
    {
        var users = await _userService.GetUsersByRoleAsync(role);
        var userResponseList = users.Select(UserMapper.ToResponse);
        return Ok(userResponseList);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> CreateUser([FromBody] UserRequest userRequest)
    {
        if (userRequest is null)
        {
            return BadRequest(new { Message = "UserRequest cannot be null" });
        }

        ValidationResult validationResult = await _validator.ValidateAsync(userRequest);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var user = UserMapper.ToUser(userRequest);

        try
        {
            var result = await _userService.CreateAsync(user);
            if (!result)
            {
                return BadRequest("Unable to create user.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the user.");
        }

        var userResponse = UserMapper.ToResponse(user);

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userResponse);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserResponse>> UpdateUser(Guid id, [FromBody] UserRequest userRequest)
    {
        if (userRequest is null)
        {
            return BadRequest("UserRequest cannot be null");
        }

        if (id == Guid.Empty)
        {
            return BadRequest("Invalid user ID.");
        }

        ValidationResult validationResult = await _validator.ValidateAsync(userRequest);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var existingUser = await _userService.GetByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound(new { Message = $"User with ID {id} not found." });
        }

        UserMapper.MapToExistingUser(userRequest, existingUser);

        try
        {
            var result = await _userService.UpdateAsync(existingUser);
            if (!result)
            {
                return BadRequest("Unable to update user.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the user.");
        }

        var userResponse = UserMapper.ToResponse(existingUser);

        return Ok(userResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid user ID.");
        }

        var existingUser = await _userService.GetByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        try
        {
            await _userService.DeleteAsync(id);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the user.");
        }

        return Ok("User deleted successfully.");
    }

    [HttpPost("authenticate")]
    public async Task<ActionResult<UserResponse>> Authenticate([FromBody] UserLoginRequest loginRequest)
    {
        if (loginRequest is null)
        {
            return BadRequest("LoginRequest cannot be null");
        }

        var user = await _userService.AuthenticateAsync(loginRequest.Username, loginRequest.Password);
        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        var userResponse = UserMapper.ToResponse(user);
        return Ok(userResponse);
    }

    [HttpPost("{id}/changePassword")]
    public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordRequest changePasswordRequest)
    {
        if (changePasswordRequest is null || id == Guid.Empty)
        {
            return BadRequest("Invalid request.");
        }

        try
        {
            var result = await _userService.ChangePasswordAsync(id, changePasswordRequest.NewPassword);
            if (!result)
            {
                return BadRequest("Unable to change password.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while changing the password.");
        }

        return Ok("Password changed successfully.");
    }
}
