using Auction.Common.Dtos.User;
using Auction.Common.Response;

namespace Auction.BLL.Interfaces;

public interface IAuthService
{
	Task<Response<UserDto>> SignInAsync(SignInUserDto userDto);
	Task<Response<UserDto>> CreateAsync(SignUpUserDto userDto);
	Task<Response<AccessTokenDto>> GenerateAccessToken(string refreshToken);
	Task<Response<string>> GenerateRefreshToken(UserDto userDto);
}
