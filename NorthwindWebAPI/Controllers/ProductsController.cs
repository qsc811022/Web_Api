using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

using NorthwindWebAPI.Models;

namespace NorthwindWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
//	今天我將介紹我們如何使用 ASP.NET Core 建立 RESTful API 來管理北風資料庫（Northwind Database）的產品資料表。這個 API 提供了標準的 CRUD（創建、讀取、更新、刪除）操作，使得前端應用能夠與後端資料庫進行互動。
//依賴注入與控制器設置:

	public class ProductsController : ControllerBase
	{
		// 依賴注入的 DbContext 實例，用於數據庫操作
//		首先，我們使用依賴注入來引入 NorthwindContext，這是 Entity Framework Core 的一部分，用於操作資料庫庫。這種方法有助於提高代碼的可測試性和靈活性。
//獲取所有產品 - GET 請求:
		private readonly NorthwindContext _context;

		// 構造函數，注入 DbContext
		public ProductsController(NorthwindContext context)
		{
			_context = context;
		}

		// GET 請求處理器，用於獲取所有產品信息
//		我們的第一個功能是獲取所有產品的列表。這通過對 /api/products 的 GET 請求來實現。我們在方法中非同步地調用 _context.Products.ToListAsync()，這樣可以提升應用的效能和響應能力。
//獲取單個產品 - GET 請求:
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			// 從數據庫非同步獲取所有產品並返回
			return await _context.Products.ToListAsync();
		}

		// GET 請求處理器，通過 ID 獲取單個產品信息
//		為了獲取單個產品的詳細資訊，我們通過 /api/products/{id
//	}
//	路徑實現了另一個 GET 請求。如果找不到對應的產品，我們返回 404 NotFound，這是 RESTful 風格的一部分，即通過 HTTP 狀態碼表達 API 的結果。
//添加新產品 - POST 請求:
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
//		接下來，我們通過 POST 請求實現了添加新產品的功能。我們將新產品添加到數據庫中，並保存更改。這裡使用PostProduct 方法返回一個指向新創建資源的響應，這符合 RESTful API 的最佳實踐。
//更新產品 - PUT 請求:
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

		//更新產品 - PUT 請求:
		//更新現有產品是通過 PUT 請求完成的。我們首先檢查提交的 ID 是否與 URL 中的 ID 匹配，然後更新數據庫中的產品信息。這裡的異常處理確保了 API 的穩定性和可靠性。
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
//		刪除產品 - DELETE 請求:

//刪除功能是通過 DELETE 請求實現的。在刪除之前，我們先檢查產品是否存在，以及是否有與之相關的訂單細節。這體現了在修改數據庫之前進行必要的業務邏輯檢查的重要性。
//確認產品存在的方法:
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
//		最後，我們有一個私有方法 ProductExists，用於在 PUT 和 DELETE 請求中檢查產品是否存在。這是一種重複代碼的減少和代碼清晰性的提高。
//結束語:
		private bool ProductExists(int id)
		{
			return _context.Products.Any(e => e.ProductId == id);
		}


	}
}
