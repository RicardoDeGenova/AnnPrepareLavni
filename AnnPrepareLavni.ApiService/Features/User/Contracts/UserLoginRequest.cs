﻿namespace AnnPrepareLavni.ApiService.Features.User.Contracts;

public class UserLoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
