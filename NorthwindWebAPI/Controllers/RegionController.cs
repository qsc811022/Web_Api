using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

using NorthwindWebAPI.Models;

namespace NorthwindWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
//	大家好，今天我將向您展示如何使用 ASP.NET Core 建立一個 RESTful API，這個 API 專門用於管理北風資料庫（Northwind Database）中的地區信息。這包括了基本的 CRUD（創建、讀取、更新、刪除）操作。
//依賴注入與控制器設置:
	public class RegionsController : ControllerBase
	{
		// 使用 NorthwindContext 來訪問數據庫
//		首先，我們使用依賴注入來引入 NorthwindContext，這是 Entity Framework Core 的一部分，用於操作資料庫庫。這種方法有助於提高代碼的可測試性和靈活性。
//獲取所有產品 - GET 請求:
		private readonly NorthwindContext _context;

		// 構造函數，透過依賴注入獲取 NorthwindContext
		public RegionsController(NorthwindContext context)
		{
			_context = context;
		}


//		我們的 API 允許用戶通過發送 GET 請求到 /api/regions 獲取所有地區的列表。在我們的方法中，我們非同步地從數據庫中獲取所有地區並返回這些信息。
//獲取特定地區 - GET 請求:
		// 處理 GET 請求到 api/regions 的路由，用於獲取所有地區
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Region>>> GetRegions()
		{
			// 非同步地從數據庫中獲取所有地區並返回
			return await _context.Regions.ToListAsync();
		}

//		為了獲取特定地區的詳細信息，我們實現了對 /api/regions/{id}
//	的 GET 請求。如果找不到相應的地區，我們返回一個 404 NotFound 響應，這符合 RESTful 風格。
//添加新地區 - POST 請求:
		// 處理 GET 請求到 api/regions/{id} 的路由，用於獲取特定 ID 的地區
		[HttpGet("{id}")]
		public async Task<ActionResult<Region>> GetRegion(int id)
		{
			// 非同步地查找具有特定 ID 的地區
			var region = await _context.Regions.FindAsync(id);

			// 如果沒有找到地區，返回 404 NotFound
			if (region == null)
			{
				return NotFound();
			}

			// 返回找到的地區
			return region;
		}

		// 處理 POST 請求到 api/regions 的路由，用於創建新地區
//		我們的 API 也支持添加新地區。這是通過向 /api/regions 發送 POST 請求實現的。在將新地區保存到數據庫後，我們返回一個指向剛創建的地區的響應，這是遵循 RESTful 最佳實踐的一部分。
//更新地區 - PUT 請求:
		[HttpPost]
		public async Task<ActionResult<Region>> PostRegion(Region region)
		{
			// 將新地區添加到數據庫
			_context.Regions.Add(region);
			// 保存更改
			await _context.SaveChangesAsync();

			// 返回創建操作的結果，包括新地區的信息
			return CreatedAtAction(nameof(GetRegion), new { id = region.RegionId}, region);
		}

		// 處理 PUT 請求到 api/regions/{id} 的路由，用於更新特定 ID 的地區
//		我們的 API 也支持添加新地區。這是通過向 /api/regions 發送 POST 請求實現的。在將新地區保存到數據庫後，我們返回一個指向剛創建的地區的響應，這是遵循 RESTful 最佳實踐的一部分。
//更新地區 - PUT 請求:
		[HttpPut("{id}")]
		public async Task<IActionResult> PutRegion(int id, Region region)
		{
			// 檢查 URL 中的 ID 是否與提供的地區 ID 匹配
			if (id != region.RegionId)
			{
				return BadRequest();
			}

			// 標記地區為已修改狀態
			_context.Entry(region).State = EntityState.Modified;
			// 保存更改
			await _context.SaveChangesAsync();

			// 返回成功但無內容的響應
			return NoContent();
		}

		// 處理 DELETE 請求到 api/regions/{id} 的路由，用於刪除特定 ID 的地區
//		刪除地區 - DELETE 請求:
//刪除地區功能是通過 DELETE 請求實現的。在從數據庫中移除地區之前，我們先檢查該地區是否存在。這是一種重要的業務邏輯檢查，確保我們不會無意中刪除不存在的數據。
//結束語:
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteRegion(int id)
		{
			// 非同步地查找要刪除的地區
			var region = await _context.Regions.FindAsync(id);
			// 如果地區不存在，返回 404 NotFound
			if (region == null)
			{
				return NotFound();
			}

			// 從數據庫中移除地區
			_context.Regions.Remove(region);
			// 保存更改
			await _context.SaveChangesAsync();

			// 返回成功但無內容的響應
			return NoContent();
			}
	}
}
