using Microsoft.AspNetCore.Mvc;

using Auction.BLL.Interfaces;
using Auction.Common.Dtos.User;
using Auction.Common.Response;
using Microsoft.Net.Http.Headers;

namespace Auction.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

	[HttpPost("sign-in")]
	public async Task<ActionResult> SignIn([FromBody] SignInUserDto userDto)
    {
        var response = await _authService.SignInAsync(userDto);

		if (response.Status == Status.Success)
        {
            var refreshToken = await _authService.GenerateRefreshToken(response.Value);
           
            Response.Cookies.Append("X-Refresh-Token", refreshToken.Value, new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None
            });

            return Ok(response);
		}

		return BadRequest(response);
    }

	[HttpPost("sign-up")]
	public async Task<ActionResult> SignUp([FromBody] SignUpUserDto userDto)
    {
		var response = await _authService.CreateAsync(userDto);

		if (response.Status == Status.Success)
		{
			var refreshToken = await _authService.GenerateRefreshToken(response.Value);
			Response.Cookies.Append("X-Refresh-Token", refreshToken.Value, new CookieOptions
			{
				Expires = DateTime.Now.AddDays(7),
				HttpOnly = true,
				Secure = true,
				IsEssential = true,
				SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None
			});

			return Ok(response);
		}
        
		return BadRequest(response);
	}

	[HttpPost("refresh")]
	public async Task<ActionResult> Refresh()
    {
		var refreshToken = Request.Cookies["X-Refresh-Token"];
		var response = await _authService.GenerateAccessToken(refreshToken);

		if (response.Status == Status.Success)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}
}
