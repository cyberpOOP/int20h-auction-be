using Auction.Common.Enums;
using Auction.DAL.Entities.Abstract;

namespace Auction.DAL.Entities;

public class Product : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public decimal? MinimalBid { get; set; }
    public string? Phone { get; set; }
    public string? ImageLinks { get; set; }
    public ProductStatus Status { get; set; }
    public DateTime? EndDate { get; set; }
    public User Seller { get; set; }
    public Guid SellerId { get; set; }
    public ICollection<Bid> Bids { get; set; } = new List<Bid>();
    public User? Winner { get; set; }
    public Guid? WinnerId { get; set; }
}
