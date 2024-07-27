using Microsoft.AspNetCore.Mvc;
using Northwind.Business.Request;
using Northwind.Business.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericService<ProductDetailRequestDTO> _productDetailService;

        public ProductsController(IGenericService<ProductDetailRequestDTO> productDetailService)
        {
            _productDetailService = productDetailService;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDetailRequestDTO>>> GetAllProducts()
        {
            var products = await _productDetailService.GetAllAsync();
            return Ok(products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetailRequestDTO>> GetProductById(int id)
        {
            try
            {
                var product = await _productDetailService.GetByIdAsync(id);
                return Ok(product);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductDetailRequestDTO>> CreateProduct(ProductDetailRequestDTO productDto)
        {
            var createdProduct = await _productDetailService.CreateAsync(productDto);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.ProductID }, createdProduct);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, ProductDetailRequestDTO productDto)
        {
            if (id != productDto.ProductID)
            {
                return BadRequest("Product ID mismatch");
            }

            try
            {
                await _productDetailService.UpdateAsync(id, productDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productDetailService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
