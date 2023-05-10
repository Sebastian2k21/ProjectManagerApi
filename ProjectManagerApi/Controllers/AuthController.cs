using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagerApi.Data.Models;
using ProjectManagerApi.Data.Repositories;
using ProjectManagerApi.Dto;
using ProjectManagerApi.Services;
using System.Security.Cryptography;

namespace ProjectManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IBaseRepository<User> userRepository;
        private readonly IMapper mapper;
        private readonly ITokenService tokenService;

        public AuthController(IBaseRepository<User> userRepository, IMapper mapper, ITokenService tokenService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if(ModelState.IsValid)
            {
                var user = mapper.Map<User>(dto);
                HMACSHA512 hmac = new HMACSHA512();
                user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dto.Password));
                user.PasswordSalt = hmac.Key;
                await userRepository.Add(user);
                return Ok();
            }

            return BadRequest("Invalid data in body");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if(ModelState.IsValid)
            {
                var user = await userRepository.FindFirst(x => x.Email == dto.Email);
                if(user == null)
                {
                    return Unauthorized();
                }
                if(!ComparePassword(user, dto.Password))
                {
                    return Unauthorized();
                }
                return Ok(new { Token = tokenService.CreateToken(user) });
            }

            return BadRequest("Invalid data in body");
        }

        [HttpGet("test")]
        [Authorize]
        public IActionResult Test()
        {
            return Ok();
        }

        private bool ComparePassword(User user, string password)
        {
            HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt);
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return user.PasswordHash.SequenceEqual(hash); //porownuje czy w obu tabliacach sa takie same elementy, bez tego trzeba by bylo uzyc petli
        }
    }
}
