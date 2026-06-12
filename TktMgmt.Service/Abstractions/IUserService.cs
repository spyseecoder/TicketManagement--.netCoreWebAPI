using System;
using System.Collections.Generic;
using System.Text;
using TktMgmt.Common.DTOs.User;

public interface IUserService
{
    Task<UserResponseDto> CreateUser(UserCreateDto dto);
    Task<string> UpdateUserRole(UserRoleUpdateDto dto);
    Task<List<UserDetailsDto>> GetUserDetails(string? roleName);
    Task<string> DeleteUser(string userGuid);
    Task<UserDetailsDto?> GetUserByGuid(string userGuid);
}
