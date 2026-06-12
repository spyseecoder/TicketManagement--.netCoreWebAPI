using System;
using System.Collections.Generic;
using System.Text;
using TktMgmt.Common.DTOs.User;
using TktMgmt.Common.Constants;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserResponseDto> CreateUser(UserCreateDto dto)
    {
        try
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var result = await _repository.CreateUser(dto, passwordHash);

            return new UserResponseDto
            {
                UserGuid = result.Item1,
                Message = result.Item2
            };
        }
        catch
        {
            return new UserResponseDto
            {
                Message = AppConstants.Error
            };
        }
    }

    public async Task<string> UpdateUserRole(UserRoleUpdateDto dto)
    {
        try
        {
            return await _repository.UpdateUserRole(dto.UserGuid, dto.RoleName);
        }
        catch
        {
            return AppConstants.Error;
        }
    }
    public async Task<List<UserDetailsDto>> GetUserDetails(string? roleName)
    {
        try
        {
            return await _repository.GetUserDetails(roleName);
        }
        catch
        {
            return new List<UserDetailsDto>();
        }
    }

    public async Task<string> DeleteUser(string userGuid)
    {
        try
        {
            return await _repository.DeleteUser(userGuid);
        }
        catch
        {
            return AppConstants.Error;
        }
    }

    public async Task<UserDetailsDto?> GetUserByGuid(string userGuid)
    {
        try
        {
            return await _repository.GetUserByGuid(userGuid);
        }
        catch
        {
            return null;
        }
    }
}
