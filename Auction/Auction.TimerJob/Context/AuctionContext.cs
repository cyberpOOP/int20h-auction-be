using Auction.TimerJob.Context.ModelConfigurations;
using Auction.TimerJob.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auction.TimerJob.Context
{
    public class AuctionContext: DbContext
    {
        public AuctionContext(DbContextOptions options) : base(options) { }
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
}
