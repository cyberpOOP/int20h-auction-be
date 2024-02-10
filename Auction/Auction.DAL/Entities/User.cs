using Auction.DAL.Entities.Abstract;

namespace Auction.DAL.Entities;

public class User : BaseEntity
{
    public string Email { get; set; }
    public string Phone { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PasswordHash { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public string AvatarUrl {  get; set; }
    public ICollection<Product> ProductSelling { get; set; }
    public ICollection<Product> WonAuctions { get; set; }
    public ICollection<Bid> Bids { get; set; }
}
