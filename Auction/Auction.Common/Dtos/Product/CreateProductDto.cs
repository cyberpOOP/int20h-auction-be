namespace Auction.Common.Dtos.Product
{
	public class CreateProductDto
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public decimal? MinimalBid { get; set; }
		public string SellerEmail { get; set; }
	}
}
