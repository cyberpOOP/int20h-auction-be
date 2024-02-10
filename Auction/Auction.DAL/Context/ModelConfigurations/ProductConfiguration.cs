using Auction.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Auction.DAL.Context.ModelConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .HasOne(p => p.Seller)
                .WithMany(u => u.ProductSelling)
                .HasForeignKey(p => p.SellerId);

            builder
                .HasOne(p => p.Winner)
                .WithMany(u => u.WonAuctions)
                .HasForeignKey(p => p.WinnerId)
                .IsRequired(false);

            builder
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");

            builder
                .Property(p => p.MinimalBid)
                .HasColumnType("decimal(18, 2)");

            builder.Property(p => p.Title)
                .IsRequired();
        }
    }
}
