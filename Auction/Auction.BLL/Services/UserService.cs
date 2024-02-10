using Auction.BLL.Interfaces;
using Auction.BLL.Services.Abstract;
using Auction.DAL.Context;
using AutoMapper;

namespace Auction.BLL.Services;

public class UserService : BaseService, IUserService
{
	public UserService(AuctionContext context, IMapper mapper) : base(context, mapper)
	{
	}
}
