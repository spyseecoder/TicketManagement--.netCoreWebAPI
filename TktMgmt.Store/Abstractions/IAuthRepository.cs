using System;
using System.Collections.Generic;
using System.Text;
using TktMgmt.Common.DTOs.Auth;

namespace TktMgmt.Store.Abstractions
{
    public interface IAuthRepository
    {
        Task<(int UserId, string UserGuid, string Email, string PasswordHash, string RoleName)> GetUserByEmail(string email);
    }
}
