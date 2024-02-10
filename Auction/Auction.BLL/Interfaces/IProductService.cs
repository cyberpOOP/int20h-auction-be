using Auction.Common.Dtos.Product;
using Auction.Common.Response;

namespace Auction.BLL.Interfaces;

public interface IProductService
{
	Task<Response<ProductDto>> CreateProduct(CreateProductDto productDto);
	Task<Response<ProductWithBidsDto>> GetProductById(Guid productId);
}
