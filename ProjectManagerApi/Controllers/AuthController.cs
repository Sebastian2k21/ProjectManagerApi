﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagerApi.Data.Models;
using ProjectManagerApi.Data.Repositories;
using ProjectManagerApi.Dto;
using ProjectManagerApi.Exceptions;
using ProjectManagerApi.Services;
using System.Security.Cryptography;

namespace ProjectManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public AuthController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    await userService.AddUser(mapper.Map<User>(dto), dto.Password, dto.TechnologiesList, dto.LanguagesList);
                    return Ok();
                }
                catch (InvalidItemIdException ex) {
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest("Invalid data in body");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    return Ok(new { Token = await userService.Auth(dto.Email, dto.Password) });
                }
                catch(InvalidCredentialsException ex)
                {
                    return Unauthorized(ex.Message);
                }
            }

            return BadRequest("Invalid data in body");
        }

        [HttpGet("test")]
        [Authorize]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}
