using Auction.Common.Dtos.Bid;
using Auction.Common.Dtos.User;
using Auction.Common.Enums;

namespace Auction.Common.Dtos.Product
{
	public class ProductWithBidsDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public string? Phone { get; set; }
		public UserDto Seller { get; set; }
		public ProductStatus Status { get; set; }
		public UserDto? Winner { get; set; }
		public DateTime? EndDate { get; set; }
		public ICollection<BidDto> Bids { get; set; }
		public IEnumerable<UserProductDto> Users { get; set; }
		public string? ImageLinks { get; set; }
		public decimal? MinimalBid { get; set; }
	}
}
