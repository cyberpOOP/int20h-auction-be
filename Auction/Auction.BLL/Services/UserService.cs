using Auction.BLL.Interfaces;
using Auction.BLL.Services.Abstract;
using Auction.Common.Dtos.User;
using Auction.Common.Response;
using Auction.DAL.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Auction.BLL.Services;

public class UserService : BaseService, IUserService
{
	public UserService(AuctionContext context, IMapper mapper) : base(context, mapper)
	{
	}

    public async Task<Response<bool>> DeleteUser(Guid userId)
    {
        var response = new Response<bool>()
        {
            Status = Status.Error,
        };

        var user = await _context.Users.FirstOrDefaultAsync(userId => userId.Id.Equals(userId)) ?? throw new ArgumentNullException("User was not found");

        _context.Users.Remove(user);

        await _context.SaveChangesAsync();

        response.Status = Status.Success;
        return response;
    }

    public async Task<Response<UserDto>> UpdateUser(SignUpUserDto userDto)
    {
        throw new NotImplementedException();
    }
}
