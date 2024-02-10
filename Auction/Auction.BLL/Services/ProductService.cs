using Auction.BLL.Interfaces;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
		var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == productDto.SellerEmail);
		if (user == null)
		{
			return new Response<ProductDto>
			{
				Message = $"Seller with email {productDto.SellerEmail} was not found",
				Status = Status.Error
			};
		}

		var product = _mapper.Map<Product>(productDto);
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
			products = products.Where(p => p.Title.Contains(filterDto.Title!, StringComparison.OrdinalIgnoreCase));
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
		if (filterDto.Skip < count)
		{
			products.Skip(filterDto.Skip.Value);
		}
		if (filterDto.Take != null && filterDto.Take < count - skip)
		{
			products.Take(filterDto.Take.Value);
		}
		return new FilterResponse<IEnumerable<ProductDto>>()
		{
			Count = count,
			Skip = skip,
			Page = filterDto.Take ?? 0,
			Value = await products.ToListAsync() as IEnumerable<ProductDto>,
			Status = Status.Success,
			Message = "Items successfully retreived!"
		};
	}
}
