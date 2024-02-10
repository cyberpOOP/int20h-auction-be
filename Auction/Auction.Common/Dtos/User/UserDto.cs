namespace Auction.Common.Dtos.User
{
	public class UserDto
	{
		public string Email { get; set; }
		public string Phone { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string AvatarUrl { get; set; }
		public string AccessToken { get; set; }
		//public ICollection<Product> ProductSelling { get; set; }
		//public ICollection<Product> WonAuctions { get; set; }
		//public ICollection<Bid> Bids { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
