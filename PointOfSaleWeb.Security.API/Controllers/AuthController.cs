﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;
using PointOfSaleWeb.Security.API.Services;

namespace PointOfSaleWeb.Security.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly AuthService _authService;

        public AuthController(IUserRepository userRepo, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _authService = new AuthService(configuration);
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult<UserInfoDTO>> Login(UserLoginDTO user)
        {
            var response = await _userRepo.GetUserData(user);

            if (!response.Success)
            {
                ModelState.AddModelError("UserError", response.Message);
                return BadRequest(ModelState);
            }

            if (response.Data != null)
            {
                response.Data.Token = _authService.CreateToken(response.Data);
            }

            return Ok(response.Data);
        }

        [HttpPost("register"), AllowAnonymous]
        public async Task<ActionResult<UserInfoDTO>> CreateUser(User user)
        {
            var response = await _userRepo.CreateUser(user);

            if (!response.Success)
            {
                ModelState.AddModelError("UserError", response.Message);
                return BadRequest(ModelState);
            }

            return Created("User", response.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateUser(int id, UserUpdateDTO user)
        {
            user.UserID = id;

            var response = await _userRepo.UpdateUser(user);

            if (!response.Success)
            {
                ModelState.AddModelError("UserError", response.Message);
                return BadRequest(ModelState);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var response = await _userRepo.DeleteUser(id);

            if (!response.Success)
            {
                ModelState.AddModelError("UserError", response.Message);
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}