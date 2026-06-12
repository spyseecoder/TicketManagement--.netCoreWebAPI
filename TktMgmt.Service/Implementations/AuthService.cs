using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TktMgmt.Common.DTOs.Auth;
using TktMgmt.Service.Abstractions;
using TktMgmt.Store.Abstractions;
using TktMgmt.Common.Constants;

namespace TktMgmt.Service.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _config;

        public AuthService(IAuthRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto dto)
        {
            try
            {
                var (userId, userGuid, emailFromDb, passwordHash, roleName) =
                await _repository.GetUserByEmail(dto.Email);

                if (userId == 0)
                    return new LoginResponseDto { Message = AppConstants.InvalidCredentials };


                // Verify password
                if (!BCrypt.Net.BCrypt.Verify(dto.Password, passwordHash))
                    return new LoginResponseDto { Message = AppConstants.InvalidCredentials };

                // Create JWT
                var claims = new[]
                {
            new Claim("UserId", userId.ToString()),
            new Claim(ClaimTypes.NameIdentifier, userGuid),
            new Claim(ClaimTypes.Email, emailFromDb),
            new Claim(ClaimTypes.Role, roleName)
            };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["Jwt:Key"])
                );

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(
                        Convert.ToDouble(_config["Jwt:ExpiryMinutes"])
                    ),
                    signingCredentials: creds
                );

                return new LoginResponseDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Message = AppConstants.Success
                };
            }
            catch
            {
                return new LoginResponseDto
                {
                    Message = AppConstants.Error
                };
            }
        }
    }
}
