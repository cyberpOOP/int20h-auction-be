namespace Auction.BLL.Interfaces
{
    public interface ICredentialService
    {
        Guid UserId { get; }
        Task<bool> SetUser(string email);
    }
}
