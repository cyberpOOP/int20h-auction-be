using Auction.Common.Dtos.File;
using Auction.Common.Dtos.User;
using Auction.Common.Response;
using Microsoft.AspNetCore.Http;

namespace Auction.BLL.Interfaces;

public interface IUserService
{
    Task<Response<UserDto>> UpdateUser(EditUserDto userDto);
    Task<Response<bool>> DeleteUser(Guid userId);
    Task<Response<UserDto>> UpdatePhoto(IFormFile file, Guid userId);
    Task<Response<UserDto>> DeletePhoto(FileDto fileDto, Guid userId);
}
