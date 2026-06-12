using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TktMgmt.Common.DTOs.User;
using Microsoft.AspNetCore.Authorization;


[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    ///<summary>
    /// Used to Create a New user 
    ///</summary>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateUser_Async([FromBody] UserCreateDto dto)
    {
        var result = await _service.CreateUser(dto);
        return Ok(result);
    }
    ///<summary>
    /// To Test if the user has logged in and is authorized to access the endpoint
    ///</summary>
    [Authorize]
    [HttpGet]
    public IActionResult Test()
    {
        return Ok("You are authorized");
    }

    ///<summary>
    /// Update User's Role
    ///</summary>
    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> UpdateUserRole_async(UserRoleUpdateDto dto)
    {
        var result = await _service.UpdateUserRole(dto);
        return Ok(result);
    }

    ///<summary>
    /// Get All User Details
    ///</summary>
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetUserDetails_async(string? roleName = null)
    {
        var result = await _service.GetUserDetails(roleName);
        return Ok(result);
    }

    ///<summary>
    /// Delete a User by User Guid
    ///</summary>
    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser_async(string userGuid)
    {
        var result = await _service.DeleteUser(userGuid);
        return Ok(result);
    }

    ///<summary>
    /// Get User Details by User Guid
    ///</summary>
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetUserByGuid_async(string userGuid)
    {
        var result = await _service.GetUserByGuid(userGuid);

        if (result == null)
            return NotFound("User not found");

        return Ok(result);
    }
}
