namespace Auction.Common.Dtos.Product
{
    public class FilterProductDto
    {
        public string? Title { get; set; }
        public string? State {  get; set; }
        public string? OrderBy { get; set; }
        public bool? OnlyWithMyBids { get; set; }
        public int? Skip { get; set; }
        public int? Take {  get; set; }
    }
}
