using Auction.Common.Dtos.User;

using Auction.Common.Response;

namespace Auction.BLL.Interfaces;

public interface IUserService
{
    Task<Response<UserDto>> UpdateUser(SignUpUserDto userDto);
    Task<Response<bool>> DeleteUser(Guid userId);
}
