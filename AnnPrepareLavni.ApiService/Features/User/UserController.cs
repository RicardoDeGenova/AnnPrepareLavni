﻿using AnnPrepareLavni.ApiService.Features.User.Contracts;
using AnnPrepareLavni.ApiService.Models;
using AnnPrepareLavni.ApiService.Utils;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnnPrepareLavni.ApiService.Features.User;

[Authorize]
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
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers(bool includePrescriptions = false, bool includeTriages = false, bool includeAppointments = false)
    {
        var users = await _userService.GetAllAsync(includePrescriptions, includeTriages, includeAppointments);
        var userResponseList = users.Select(UserMapper.ToResponse);
        return Ok(userResponseList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> GetUser(Guid id, bool includePrescriptions = false, bool includeTriages = false, bool includeAppointments = false)
    {
        if (id == Guid.Empty)
        {
            return BadRequest(new ReturnMessage("Invalid user ID."));
        }

        var user = await _userService.GetByIdAsync(id, includePrescriptions, includeTriages, includeAppointments);
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
                return BadRequest(new ReturnMessage("Unable to create user."));
            }
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ReturnMessage("An error occurred while creating the user."));
        }

        var userResponse = UserMapper.ToResponse(user);

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userResponse);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserResponse>> UpdateUser(Guid id, [FromBody] UserRequest userRequest)
    {
        if (userRequest is null)
        {
            return BadRequest(new ReturnMessage("UserRequest cannot be null"));
        }

        if (id == Guid.Empty)
        {
            return BadRequest(new ReturnMessage("Invalid user ID."));
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
                return BadRequest(new ReturnMessage("Unable to update user."));
            }
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ReturnMessage("An error occurred while updating the user."));
        }

        var userResponse = UserMapper.ToResponse(existingUser);

        return Ok(userResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest(new ReturnMessage("Invalid user ID."));
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
            return StatusCode(StatusCodes.Status500InternalServerError, new ReturnMessage("An error occurred while deleting the user."));
        }

        return Ok(new ReturnMessage("User deleted successfully."));
    }

    [HttpPost("{id}/changePassword")]
    public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordRequest changePasswordRequest)
    {
        if (changePasswordRequest is null || id == Guid.Empty)
        {
            return BadRequest(new ReturnMessage("Invalid request."));
        }

        try
        {
            var result = await _userService.ChangePasswordAsync(id, changePasswordRequest.NewPassword);
            if (!result)
            {
                return BadRequest(new ReturnMessage("Unable to change password."));
            }
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ReturnMessage("An error occurred while changing the password."));
        }

        return Ok(new ReturnMessage("Password changed successfully."));
    }
}
