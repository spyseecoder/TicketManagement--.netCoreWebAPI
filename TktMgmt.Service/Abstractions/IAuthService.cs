using System;
using System.Collections.Generic;
using System.Text;
using TktMgmt.Common.DTOs.Auth;


namespace TktMgmt.Service.Abstractions
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginRequestDto dto);
    }
}
