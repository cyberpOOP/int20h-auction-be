using Auction.DAL.Context.ModelConfigurations;
using Auction.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auction.DAL.Context;

public class AuctionContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Bid> Bids { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new BidConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
