using Auction.DAL.Context;
using Auction.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Auction.DAL.Helpers
{
	public class MigrationHelper : IMigrationHelper
	{
		private readonly AuctionContext _context;
        public MigrationHelper(AuctionContext context)
        {
            _context = context;
        }
        public void Migrate()
		{
			if (_context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Count() > 0)
			{
				_context.Database.Migrate();
			}
		}
	}
}
