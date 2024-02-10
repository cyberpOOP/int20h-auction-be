using Auction.BLL.Interfaces;
using Auction.Common.Dtos.Bid;
using Auction.Common.Dtos.Product;
using Auction.Common.Response;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateProductDto productDto)
    {
        var response = await _productService.CreateProduct(productDto);

        if(response.Status == Status.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
    [HttpPost]
    public async Task<ActionResult> Get([FromBody] FilterProductDto filterDto)
    {
        var response = await _productService.GetProducts(filterDto);

        if (response.Status == Status.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
}
