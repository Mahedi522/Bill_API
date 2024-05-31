using Bill_API.Models;
using Bill_API.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bill_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductAPIController : ControllerBase
	{
		private readonly IProductRepository productRepository;

		public ProductAPIController(IProductRepository productRepository)
		{
			this.productRepository = productRepository;
		}

		[HttpGet(nameof(GetProducts))]
		public async Task<ActionResult<List<Product>>> GetProducts()
		{
			var products = await productRepository.GetProducts();
			return Ok(products);
		}

		[HttpGet(nameof(GetProductById) + "/{id}")]
		public async Task<ActionResult<Product>> GetProductById(int id)
		{
			var product = await productRepository.GetProductById(id);
			if (product == null)
			{
				return NotFound();
			}
			return Ok(product);
		}

		[HttpPost(nameof(CreateProduct))]
		public async Task<ActionResult<Product>> CreateProduct(Product product)
		{
			var createdProduct = await productRepository.CreateProduct(product);
			return Ok(createdProduct);
		}

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> EditProduct(int id, Product product)
		{
			if (id != product.Id)
			{
				return BadRequest();
			}

			var updatedProduct = await productRepository.EditProduct(id, product);
			return Ok(updatedProduct);
		}

		[HttpDelete(nameof(DeleteProduct) + "/{id}")]
		public async Task<ActionResult> DeleteProduct(int id)
		{
			var result = await productRepository.DeleteProduct(id);
			if (!result)
			{
				return BadRequest();
			}
			return Ok();
		}
        [HttpGet(nameof(GetProductNames))]
        public async Task<ActionResult<ProductNames>> GetProductNames()
        {
            var pNames = await productRepository.GetProductNames();
            return Ok(pNames);
        }
    }
}
