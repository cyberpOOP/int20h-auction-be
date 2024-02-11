using Auction.BLL.Interfaces;
using Auction.BLL.Services;
using Auction.Common.Dtos.Bid;
using Auction.Common.Dtos.User;
using Auction.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ICredentialService _credentialService;

    public UserController(IUserService userService, ICredentialService credentialService)
    {
        _userService = userService;
        _credentialService = credentialService;
    }

    [HttpPut]
    public async Task<ActionResult> EditProfile([FromBody] SignUpUserDto userDto)
    {
        var response = await _userService.UpdateUser(userDto);

        if (response.Status == Status.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteProfile()
    {
        var id = _credentialService.UserId;
        var response = await _userService.DeleteUser(id);

        if (response.Status == Status.Success)
        {
            return NoContent();
        }

        return BadRequest(response);
    }
}
