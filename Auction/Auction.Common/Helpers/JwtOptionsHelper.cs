namespace Auction.Common.Helpers;

public class JwtOptionsHelper
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public int TokenExpiration { get; set; }
}
