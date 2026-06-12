using System;
using System.Collections.Generic;
using System.Text;
using TktMgmt.Common.DTOs.User;

public interface IUserRepository
{
    Task<(string, string)> CreateUser(UserCreateDto dto, string passwordHash);
    Task<string> UpdateUserRole(string userGuid, string roleName);
    Task<List<UserDetailsDto>> GetUserDetails(string? roleName);
    Task<string> DeleteUser(string userGuid);
    Task<UserDetailsDto?> GetUserByGuid(string userGuid);
}
