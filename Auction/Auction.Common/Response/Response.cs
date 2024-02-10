namespace Auction.Common.Response;

public class Response<T> where T : class
{
    public T? Value { get; set; }
    public string? Message { get; set; }
    public Status Status { get; set; }
}
