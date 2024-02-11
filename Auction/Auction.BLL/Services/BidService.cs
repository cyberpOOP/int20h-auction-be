using Auction.BLL.Interfaces;
using Auction.BLL.Services.Abstract;
using Auction.Common.Dtos.Bid;
using Auction.Common.Dtos.Product;
using Auction.Common.Response;
using Auction.DAL.Context;
using Auction.DAL.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Auction.BLL.Services;

public class BidService : BaseService, IBidService
{
	private readonly ICredentialService _credentialService;
	public BidService(AuctionContext context, IMapper mapper, ICredentialService credentialService) : base(context, mapper)
	{
		_credentialService = credentialService;
	}

	public async Task<Response<ProductWithBidsDto>> CreateBid(CreateBidDto bidDto)
	{
		var userId = _credentialService.UserId;
		var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
		if (user == null)
		{
			return new Response<ProductWithBidsDto>
			{
				Message = "Error with user",
				Status = Status.Error
			};
		}

		var product = await _context.Products
			.Include(p => p.Bids)
			.ThenInclude(b => b.Bidder)
			.FirstOrDefaultAsync(p => p.Id == bidDto.ProductId);
		if (product == null)
		{
			return new Response<ProductWithBidsDto>
			{
				Message = $"Product with id {bidDto.ProductId} was not found",
				Status = Status.Error
			};
		}

		if (product.Price >= bidDto.Price)
		{
			return new Response<ProductWithBidsDto>
			{
				Message = $"You cannot place a bid that less or equal current bid",
				Status = Status.Error
			};
		}

		var bid = _mapper.Map<Bid>(bidDto);
		bid.Bidder = user;
		bid.CreatedAt = DateTime.UtcNow;
		bid.UpdatedAt = DateTime.UtcNow;

		product.Price = bidDto.Price;
		product.Bids.Add(bid);
		product.UpdatedAt = DateTime.UtcNow;

		await _context.Bids.AddAsync(bid);
		_context.Products.Update(product);

		await _context.SaveChangesAsync();

		return new Response<ProductWithBidsDto>
		{
			Value = _mapper.Map<ProductWithBidsDto>(product),
			Message = $"You successfuly placed new bid",
			Status = Status.Success
		};
	}
}
