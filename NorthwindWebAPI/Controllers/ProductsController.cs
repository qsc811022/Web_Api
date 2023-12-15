using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using NorthwindWebAPI.Models;

namespace NorthwindWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly NorthwindContext _context;

		public ProductsController(NorthwindContext context)
		{
			_context = context;
		}

		// GET: api/products
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			return await _context.Products.ToListAsync();
		}

		// GET: api/products/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var product = await _context.Products.FindAsync(id);

			if (product == null)
			{
				return NotFound();
			}

			return product;
		}

		// POST: api/products
		[HttpPost]
		public async Task<ActionResult<Product>> PostProduct(Product product)
		{
			_context.Products.Add(product);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
		}

		// PUT: api/products/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutProduct(int id, Product product)
		{
			if (id != product.ProductId)
			{
				return BadRequest();
			}

			_context.Entry(product).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProductExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}
		// DELETE: api/products/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null)
			{
				return NotFound();
			}

			// 檢查是否存在相關的 Order Details
			if (_context.OrderDetails.Any(od => od.ProductId == id))
			{
				return BadRequest("無法刪除：該產品與一個或多個訂單細節相關聯。");
			}

			_context.Products.Remove(product);
			await _context.SaveChangesAsync();

			return NoContent();
		}
		private bool ProductExists(int id)
		{
			return _context.Products.Any(e => e.ProductId == id);
		}


	}
}
