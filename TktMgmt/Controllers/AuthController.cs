using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TktMgmt.Common.DTOs.Auth;
using TktMgmt.Service.Abstractions;

namespace TktMgmt.API.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        ///<summary>
        /// Used to Log in a User
        ///</summary>
        [HttpPost]
        public async Task<IActionResult> User_Login_Async(LoginRequestDto dto)
        {
            var result = await _service.Login(dto);
            return Ok(result);
        }
    }
}
