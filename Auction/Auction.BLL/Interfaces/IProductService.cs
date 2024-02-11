using Auction.Common.Dtos.Product;
using Auction.Common.Dtos.User;
using Auction.Common.Response;

namespace Auction.BLL.Interfaces;

public interface IProductService
{
	Task<Response<ProductDto>> CreateProduct(CreateProductDto productDto);
	Task<Response<ProductWithBidsDto>> GetProductById(Guid productId);
    Task<Response<IEnumerable<ProductDto>>> GetProducts(FilterProductDto filterDto);
	Task<Response<ProductDto>> UpdateProduct(Guid productId, CreateProductDto productDto);
	Task<Response<List<UserDto>>> GetParticipators(Guid productId);
}
