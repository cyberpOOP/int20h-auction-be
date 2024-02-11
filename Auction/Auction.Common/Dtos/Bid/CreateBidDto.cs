namespace Auction.Common.Dtos.Bid
{
    public class CreateBidDto
    {
		public Guid ProductId { get; set; }
		public decimal Price { get; set; }
	}
}
