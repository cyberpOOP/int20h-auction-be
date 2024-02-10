using Auction.Common.Dtos.Bid;
using Auction.Common.Dtos.Product;
using Auction.Common.Response;

namespace Auction.BLL.Interfaces;

public interface IBidService
{
	Task<Response<ProductDto>> CreateBid(CreateBidDto bidDto);
}
