using Auction.TimerJob.Entities.Abstract;
using System.Collections.Generic;

namespace Auction.TimerJob.Entities;

public class User : BaseEntity
{
    public string Email { get; set; }
    public string Phone { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PasswordHash { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public string? AvatarUrl {  get; set; }
    public ICollection<Product>? ProductSelling { get; set; } = new List<Product>();
    public ICollection<Product>? WonAuctions { get; set; } = new List<Product>();
    public ICollection<Bid>? Bids { get; set; } = new List<Bid>();
}
