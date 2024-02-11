using Auction.Common.Enums;

namespace Auction.Common.Dtos.Product
{
	public class EditProductDto
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public decimal? MinimalBid { get; set; }
		public string? ImageLinks { get; set; }
		public DateTime? EndDate { get; set; }
		public ProductStatus? Status { get; set; }
	}
}
