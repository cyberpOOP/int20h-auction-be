using System;
using System.Linq;
using Auction.TimerJob.Context;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Auction.TimerJob
{
    public class Functions
    {
        private readonly AuctionContext _context;
        public Functions(AuctionContext context)
        {
            _context = context;
        }
        [FunctionName("AuctionTimerJob")]
        public void Run([TimerTrigger("0 0 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Auction Timer trigger function executed at: {DateTime.Now}");

            var products = _context.Products
                .Include(p => p.Bids)
                .Where(p => p.EndDate <= DateTime.Today)
                .ToList();

            foreach ( var product in products ) 
            {
                try
                {
                    if (!product.Bids.Any()) 
                    {
                        product.Status = Enums.ProductStatus.Closed;
                        log.LogInformation($"Product with id {product.Id} is closed.");
                        _context.SaveChanges();
                    }
                    else
                    {
                        var maxBid = _context.Bids
                        .OrderByDescending(b => b.Price)
                        .FirstOrDefault();

                        product.WinnerId = maxBid.BidderId;
                        product.Status = Enums.ProductStatus.Sold;
                        _context.SaveChanges();

                        log.LogInformation($"Product with id {product.Id} is sold.");
                    }
                }
                catch ( Exception ex )
                {
                    log.LogInformation($"Product with id {product.Id} cannot be proccessed. Exception: {ex.Message}");
                }
            }
        }
    }
}
