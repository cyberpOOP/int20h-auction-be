﻿using Auction.TimerJob.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.TimerJob.Context.ModelConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .IsRequired();

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder
                .HasMany(u => u.ProductSelling)
                .WithOne(p => p.Seller)
                .HasForeignKey(p => p.SellerId);

            builder
                .HasMany(u => u.WonAuctions)
                .WithOne(p => p.Winner)
                .HasForeignKey(p => p.WinnerId)
                .IsRequired(false);
        }
    }
}
