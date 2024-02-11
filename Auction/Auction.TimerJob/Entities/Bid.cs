﻿using Auction.TimerJob.Entities.Abstract;
using System;

namespace Auction.TimerJob.Entities;

public class Bid : BaseEntity
{
    public User Bidder { get; set; }
    public Guid BidderId { get; set; }
    public Product Product { get; set; }
    public Guid ProductId { get; set; }
    public decimal Price { get; set; }
}
