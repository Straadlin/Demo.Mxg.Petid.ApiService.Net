﻿namespace Mxg.Petid.ApiService.Net.Application.Common.Models.Identity;

public class AuthRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}