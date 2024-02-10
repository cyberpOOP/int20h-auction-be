using Auction.Common.Dtos.User;

namespace Auction.Common.Dtos.Bid
{
	public class BidDto
	{
		public UserDto Bidder { get; set; }
		public decimal Price { get; set; }
	}
}
