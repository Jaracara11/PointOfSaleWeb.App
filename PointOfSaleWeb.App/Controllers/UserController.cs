﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PointOfSaleWeb.App.Utilities;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;

namespace PointOfSaleWeb.App.Controllers;

[Route("api/users")]
[ApiController]
public class UserController(IUserRepository userRepo) : ControllerBase
{
    private readonly IUserRepository _userRepo = userRepo;

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ResponseCache(Duration = 5)]
    public async Task<IResult> GetAllUsers() =>
        Results.Ok(await _userRepo.GetAllUsers());

    [HttpGet("{username}")]
    [Authorize(Roles = "Admin")]
    [ResponseCache(Duration = 5)]
    public async Task<IResult> GetUserByUsername(string username)
    {
        var user = await _userRepo.GetUserByUsername(username);

        return user != null
            ? Results.Ok(user)
            : ResponseUtil.CreateNotFoundResponse(
                "User Not Found",
                $"No user found with username {username}."
            );
    }

    [HttpGet("roles"), AllowAnonymous]
    [ResponseCache(Duration = 43200)]
    public async Task<IResult> GetAllUserRoles() =>
        Results.Ok(await _userRepo.GetAllUserRoles());

    [HttpPost("auth"), AllowAnonymous]
    public async Task<IResult> AuthUser([FromBody] UserAuthDTO user)
    {
        var response = await _userRepo.AuthUser(user);

        if (response?.UserID == null)
        {
            return ResponseUtil.CreateErrorResponse(
                "Authentication Failed",
                response?.Message ?? "Authentication failed for unknown reasons.",
                StatusCodes.Status400BadRequest
            );
        }

        return Results.Ok(new UserInfoDTO
        {
            Username = response.Username,
            Name = $"{response.FirstName} {response.LastName}",
            Email = response.Email,
            Role = response.RoleName,
            Token = CreateToken(response.RoleName)
        });
    }

    [HttpPost("register"), AllowAnonymous]
    public async Task<IResult> CreateUser([FromBody] UserInsertDTO user)
    {
        var success = await _userRepo.AddNewUser(user);

        return success
            ? Results.Created("/api/users", user)
            : ResponseUtil.CreateErrorResponse(
                "Registration Failed",
                "User registration failed due to unknown error.",
                StatusCodes.Status400BadRequest
            );
    }

    [HttpPut("edit")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> UpdateUser([FromBody] UserDataDTO user)
    {
        var response = await _userRepo.UpdateUser(user);

        return response != null
            ? Results.Ok(response)
            : ResponseUtil.CreateErrorResponse(
                "Update Failed",
                "Failed to update user.",
                StatusCodes.Status400BadRequest
            );
    }

    [HttpPut("change-password")]
    [Authorize]
    public async Task<IResult> ChangeUserPassword([FromBody] UserChangePasswordDTO userData)
    {
        var response = await _userRepo.ChangeUserPassword(userData);

        return response
            ? Results.NoContent()
            : ResponseUtil.CreateErrorResponse(
                "Password Change Failed",
                "Failed to change password.",
                StatusCodes.Status400BadRequest
            );
    }

    [HttpPut("new-password")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> ResetUserPassword([FromBody] UserChangePasswordDTO userData)
    {
        var response = await _userRepo.ResetUserPassword(userData);

        return response
            ? Results.NoContent()
            : ResponseUtil.CreateErrorResponse(
                "Password Reset Failed",
                "Failed to reset password.",
                StatusCodes.Status400BadRequest
            );
    }

    [HttpDelete("{username}/delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> DeleteUser(string username)
    {
        var response = await _userRepo.DeleteUser(username);

        return response
            ? Results.NoContent()
            : ResponseUtil.CreateErrorResponse(
                "Deletion Failed",
                "Failed to delete user.",
                StatusCodes.Status400BadRequest
            );
    }

    private static string CreateToken(string userRole)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Role, userRole)
        };

        var jwtKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? string.Empty));

        var credentials = new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(12),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}