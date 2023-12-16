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
		// 依賴注入的 DbContext 實例，用於數據庫操作
		private readonly NorthwindContext _context;

		// 構造函數，注入 DbContext
		public ProductsController(NorthwindContext context)
		{
			_context = context;
		}

		// GET 請求處理器，用於獲取所有產品信息
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			// 從數據庫非同步獲取所有產品並返回
			return await _context.Products.ToListAsync();
		}

		// GET 請求處理器，通過 ID 獲取單個產品信息
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			// 非同步查找指定 ID 的產品
			var product = await _context.Products.FindAsync(id);

			// 如果產品不存在，返回 404 NotFound
			if (product == null)
			{
				return NotFound();
			}

			// 返回找到的產品
			return product;
		}

		// POST 請求處理器，用於添加新的產品
		[HttpPost]
		public async Task<ActionResult<Product>> PostProduct(Product product)
		{
			// 向數據庫添加新產品
			_context.Products.Add(product);
			// 保存更改
			await _context.SaveChangesAsync();

			// 返回創建成功的響應，包括新產品的信息
			return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
		}

		// PUT 請求處理器，用於更新現有產品
		[HttpPut("{id}")]
		public async Task<IActionResult> PutProduct(int id, Product product)
		{
			// 檢查 URL 中的 ID 是否與產品 ID 匹配
			if (id != product.ProductId)
			{
				return BadRequest();
			}

			// 標記產品為已修改狀態
			_context.Entry(product).State = EntityState.Modified;

			try
			{
				// 儲存更改
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				// 檢查產品是否存在
				if (!ProductExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			// 返回成功但無內容的響應
			return NoContent();
		}

		// DELETE 請求處理器，用於刪除產品
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			// 非同步查找要刪除的產品
			var product = await _context.Products.FindAsync(id);
			// 如果產品不存在，返回 404 NotFound
			if (product == null)
			{
				return NotFound();
			}

			// 檢查產品是否與訂單細節相關聯
			if (_context.OrderDetails.Any(od => od.ProductId == id))
			{
				// 如果相關聯，返回錯誤訊息，禁止刪除
				return BadRequest("無法刪除：該產品與一個或多個訂單細節相關聯。");
			}

			// 從數據庫中移除產品
			_context.Products.Remove(product);
			// 保存更改
			await _context.SaveChangesAsync();

			// 返回成功但無內容的響應
			return NoContent();
		}

		// 私有方法，用於檢查特定 ID 的產品是否存在
		private bool ProductExists(int id)
		{
			return _context.Products.Any(e => e.ProductId == id);
		}


	}
}
