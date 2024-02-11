using Auction.BLL.Interfaces;
using Auction.BLL.Services;
using Auction.Common.Dtos.Bid;
using Auction.Common.Dtos.Product;
using Auction.Common.Response;
using Microsoft.AspNetCore.Authorization;
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
	[Authorize]
	public async Task<ActionResult> Create([FromBody] CreateProductDto productDto)
	{
		var response = await _productService.CreateProduct(productDto);

		if (response.Status == Status.Success)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}
	[HttpPost("Get")]
	public async Task<ActionResult> Get([FromBody] FilterProductDto filterDto)
	{
		var response = await _productService.GetProducts(filterDto);

		if (response.Status == Status.Success)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult> GetById(Guid id)
	{
		var response = await _productService.GetProductById(id);

		if (response.Status == Status.Success)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	[HttpPut("{id}")]
	public async Task<ActionResult> Update(Guid id, [FromBody] CreateProductDto productDto)
	{
		var response = await _productService.UpdateProduct(id, productDto);

		if (response.Status == Status.Success)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	[HttpGet("{id}/participators")]
	public async Task<ActionResult> GetParticipators(Guid id)
	{
		var response = await _productService.GetParticipators(id);

		if (response.Status == Status.Success)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

    [HttpPost("addPhoto")]
    public async Task<ActionResult> AddPhoto()
    {
        var formCollection = await Request.ReadFormAsync();
        var file = formCollection.Files.FirstOrDefault();

        if (file is not null && file.Length > 0)
        {
            var result = await _productService.AddPhoto(file);
            return Ok(result);
        }

        return BadRequest("No files found in the request.");
    }
}
