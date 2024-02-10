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

namespace Auction.BLL.Services;

public class ProductService : BaseService, IProductService
{
	public ProductService(AuctionContext context, IMapper mapper) : base(context, mapper) { }

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
}
