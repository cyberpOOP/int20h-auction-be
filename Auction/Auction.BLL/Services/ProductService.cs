﻿using Auction.BLL.Interfaces;
using Auction.BLL.Services.Abstract;
using Auction.Common.Dtos.Bid;
using Auction.Common.Dtos.Product;
using Auction.Common.Enums;
using Auction.Common.Response;
using Auction.DAL.Context;
using Auction.DAL.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Auction.BLL.Services;

public class ProductService : BaseService, IProductService
{
	private readonly ICredentialService _credentialService;
	public ProductService(
		AuctionContext context, 
		IMapper mapper,
		ICredentialService credentialService) : base(context, mapper) 
	{
		_credentialService = credentialService;
	}

	public async Task<Response<ProductDto>> CreateProduct(CreateProductDto productDto)
	{
		if(productDto.EndDate <= DateTime.UtcNow)
		{
			return new Response<ProductDto>
			{
				Message = "Wrong end date",
				Status = Status.Error
			};
		}

		var userId = _credentialService.UserId;
		var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
		if (user == null)
		{
			return new Response<ProductDto>
			{
				Message = "Error with user",
				Status = Status.Error
			};
		}

		var product = _mapper.Map<Product>(productDto);
		if(productDto.MinimalBid != null)
		{
			product.Price = productDto.MinimalBid.Value;
		}

		product.Seller = user;
		product.Phone = user.Phone;
		product.Status = ProductStatus.Active;
		product.CreatedAt = DateTime.UtcNow;
		product.UpdatedAt = DateTime.UtcNow;

		await _context.Products.AddAsync(product);
		await _context.SaveChangesAsync();

		return new Response<ProductDto>()
		{
			Value = _mapper.Map<ProductDto>(product),
			Message = "Product created succesfully",
			Status = Status.Success
		};
	}

	public async Task<Response<ProductWithBidsDto>> GetProductById(Guid productId)
	{
		var product = await _context.Products
			.Include(p => p.Bids)
			.ThenInclude(b => b.Bidder)
			.FirstOrDefaultAsync(p => p.Id == productId);
		if (product == null)
		{
			return new Response<ProductWithBidsDto>
			{
				Message = $"Product with id {productId} was not found",
				Status = Status.Error
			};
		}

		return new Response<ProductWithBidsDto>
		{
			Value = _mapper.Map<ProductWithBidsDto>(product),
			Message = $"Product fetched succesfuly",
			Status = Status.Success
		};
	}

	public async Task<Response<IEnumerable<ProductDto>>> GetProducts(FilterProductDto filterDto)
	{
		var products = _context.Products.AsNoTracking().AsQueryable();
		if (!filterDto.State.IsNullOrEmpty())
		{
			if (!Enum.TryParse(typeof(ProductStatus), filterDto.State, true, out var state))
			{
				return new Response<IEnumerable<ProductDto>>()
				{
					Message = $"Invalid state argument",
					Status = Status.Error
				};
			}
			products = products.Where(p => p.Status == (ProductStatus)state);
		}
		if (!filterDto.Title.IsNullOrEmpty())
		{
			products = products.Where(p => p.Title.ToLower().Contains(filterDto.Title!.ToLower()));
		}
		if (!filterDto.OnlyWithMyBids != null && filterDto.OnlyWithMyBids == true)
		{
			products = products.Where(p => p.Bids.Any(b => b.BidderId == _credentialService.UserId));
		}
		if (!filterDto.OrderBy.IsNullOrEmpty())
		{
			switch (filterDto.OrderBy)
			{
                case "Title":
                    products = products.OrderBy(p => p.Title);
                    break;
                case "Price":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "MinimalBid":
                    products = products.OrderBy(p => p.MinimalBid);
                    break;
                case "EndDate":
                    products = products.OrderBy(p => p.EndDate);
                    break;
                default:
                    return new Response<IEnumerable<ProductDto>>()
                    {
                        Message = $"Invalid OrderBy argument",
                        Status = Status.Error
                    };
            };
		}
        var count = products.Count();
        var skip = filterDto.Skip ?? 0;

        if (skip < count)
        {
            products = products.Skip(skip);
        }

        if (filterDto.Take != null && filterDto.Take < count - skip)
        {
            products = products.Take(filterDto.Take.Value);
        }

        var productsList = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(await products.ToListAsync());

        var totalPages = (int)Math.Ceiling((double)count / (filterDto.Take ?? 1));
        var currentPage = (int)Math.Floor((double)skip / (filterDto.Take ?? 1)) + 1;

        return new FilterResponse<IEnumerable<ProductDto>>()
        {
            Count = count,
            Skip = skip,
            Page = totalPages > 0 ? currentPage : 1,
            Value = productsList,
            Status = Status.Success,
            Message = "Items successfully retrieved!"
        };

    }
}
