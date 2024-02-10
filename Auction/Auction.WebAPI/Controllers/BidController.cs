using Auction.BLL.Interfaces;
using Auction.Common.Dtos.Bid;
using Auction.Common.Response;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BidController : ControllerBase
{
	private readonly IBidService _bidService;

	public BidController(IBidService bidService)
	{
		_bidService = bidService;
	}

	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateBidDto bidDto)
	{
		var response = await _bidService.CreateBid(bidDto);

		if(response.Status == Status.Success)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}
}
