using Auction.BLL.Interfaces;
using Auction.BLL.Services.Abstract;
using Auction.Common.Dtos.User;
using Auction.Common.Response;
using Auction.DAL.Context;
using Auction.DAL.Entities;
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

        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id.Equals(userId)) ?? throw new ArgumentNullException("User was not found");

        _context.Users.Remove(user);

        await _context.SaveChangesAsync();

        response.Status = Status.Success;
        return response;
    }

    public async Task<Response<UserDto>> UpdateUser(EditUserDto userDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);

        if (user is null)
        {
            return new Response<UserDto>
            {
                Message = $"User with email {userDto.Email} not found",
                Status = Status.Error
            };
        }

        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.Email = userDto.Email;
        user.Phone = userDto.Phone;
        user.UpdatedAt = DateTime.UtcNow;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        var userResponse = _mapper.Map<UserDto>(user);

        return new Response<UserDto>()
        {
            Value = userResponse,
            Message = "You have updated user succesfully",
            Status = Status.Success
        };
    }
}
