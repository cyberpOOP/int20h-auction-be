using Auction.BLL.Interfaces;
using Auction.BLL.Services.Abstract;
using Auction.DAL.Context;
using AutoMapper;

namespace Auction.BLL.Services;

public class ProductService : BaseService, IProductService
{
	public ProductService(AuctionContext context, IMapper mapper) : base(context, mapper)
	{
	}
}
