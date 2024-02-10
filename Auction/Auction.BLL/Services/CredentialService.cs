using Auction.BLL.Interfaces;
using Auction.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Auction.BLL.Services
{
    public class CredentialService(AuctionContext context): ICredentialService
    {
        private readonly AuctionContext _context = context;
        private Guid userId;

        public Guid UserId => userId;
        public async Task<bool> SetUser(string email)
        {
            if (email.IsNullOrEmpty())
            {
                return false;
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return false;                
            }
            userId = user.Id;
            return true;
        }
    }
}
