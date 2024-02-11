namespace Auction.Common.Response
{
    public class FilterResponse<T> : Response<T> where T : class
    {
        public int Count {  get; set; }
        public int Page {  get; set; }
        public int Skip { get; set; }
    }
}
