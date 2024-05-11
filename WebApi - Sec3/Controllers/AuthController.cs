﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using WebApi___Sec3.DTOs;
using WebApi___Sec3.Models;
using WebApi___Sec3.Services;

namespace WebApi___Sec3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;


        public AuthController(ITokenService tokenService,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }



        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole(string rolename)
        {
            var roleExist = await _roleManager.RoleExistsAsync(rolename);

            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(rolename));

                if (roleResult.Succeeded)
                {
                    _logger.LogInformation(1, "Roles Added");
                    return StatusCode(StatusCodes.Status200OK, new Response
                    {
                        Status = "Success",
                        Message =
                        $"Role {rolename} added successfully"
                    });
                }
                else
                {
                    _logger.LogInformation(2, "Error");
                    return StatusCode(StatusCodes.Status400BadRequest, new Response
                    {
                        Status = "Error",
                        Message =
                        $"Issue addind the new {rolename} role"
                    });
                }

            }

            return StatusCode(StatusCodes.Status400BadRequest, new Response
            {
                Status = "Error",
                Message =
                        $"Role already exist."
            });



        }







        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user  = await _userManager.FindByNameAsync(model.UserName!);//Buscar user


            //Notar se ele foi ou nao encontrado
            if(user is not null && await _userManager.CheckPasswordAsync(user , model.Password!)) 
            {
            
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshtokenValidityInMinutes);

                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshtokenValidityInMinutes);

                user.RefreshToken = refreshToken;

                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }

            return Unauthorized();

        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.UserName!);

            if(userExists != null) //Verificar se o usuario existe
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "user already exists!" });
            }

            ApplicationUser user = new() //Caso nao existe vai criar uma instancia de AppUser
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
            };

            var result = await _userManager.CreateAsync(user, model.Password!);//

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "User creation failed" });
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully" });


        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if(tokenModel is null)
                return BadRequest("Invalid client request");

            string? accessToken = tokenModel.AccessToken 
                ?? throw new ArgumentNullException(nameof(tokenModel));

            string? refreshToken= tokenModel.RefreshToken 
                ?? throw new ArgumentException(nameof(tokenModel));

            var principal = _tokenService.GetPrincipalExpiredToken(accessToken!, _configuration);

            if (principal is null)
                return BadRequest("Invalid access token/refresh token");

            string username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username!);

            if(user == null || user.RefreshToken != refreshToken
                || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token/refresh token");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(
                principal.Claims.ToList(), _configuration);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]

        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return BadRequest("Invalid username");

            user.RefreshToken = null;
            
            await _userManager.UpdateAsync(user);   

            return NoContent();
        }






    }
}
