namespace Auction.Common.Dtos.Bid
{
    public class CreateBidDto
    {
		public string BidderEmail { get; set; }
		public Guid ProductId { get; set; }
		public decimal Price { get; set; }
	}
}
