using Auction.DAL.Context;
using AutoMapper;

namespace Auction.BLL.Services.Abstract;

public abstract class BaseService
{
	protected readonly AuctionContext _context;
	protected readonly IMapper _mapper;

	protected BaseService(AuctionContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}
}
