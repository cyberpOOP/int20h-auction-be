using Auction.BLL.Interfaces;
using Auction.Common.Dtos.File;
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
    public async Task<ActionResult> EditProfile([FromBody] EditUserDto userDto)
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
        var response = await _userService.DeleteUser(_credentialService.UserId);

        if (response.Status == Status.Success)
        {
            return NoContent();
        }

        return BadRequest(response);
    }

    [HttpPost("addPhoto")]
    public async Task<ActionResult> AddPhoto()
    {
        var formCollection = await Request.ReadFormAsync();
        var file = formCollection.Files.FirstOrDefault();

        if (file is not null && file.Length > 0)
        {
            var result = await _userService.UpdatePhoto(file, _credentialService.UserId);
            return Ok(result);
        }

        return BadRequest("No files found in the request.");
    }

    [HttpDelete("deletePhoto")]
    public async Task<ActionResult> DeletePhoto([FromBody] FileDto fileDto)
    {
        if (fileDto is not null)
        {
            var result = await _userService.DeletePhoto(fileDto, _credentialService.UserId);
            return Ok(result);
        }

        return BadRequest("No file url.");
    }
}
