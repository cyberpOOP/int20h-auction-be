using Auction.Common.Dtos.User;

using Auction.Common.Response;

namespace Auction.BLL.Interfaces;

public interface IUserService
{
    Task<Response<UserDto>> UpdateUser(EditUserDto userDto);
    Task<Response<bool>> DeleteUser(Guid userId);
}
