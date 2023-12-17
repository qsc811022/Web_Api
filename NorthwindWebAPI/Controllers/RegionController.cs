﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using NorthwindWebAPI.Models;

namespace NorthwindWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegionsController : ControllerBase
	{
		// 使用 NorthwindContext 來訪問數據庫
		private readonly NorthwindContext _context;

		// 構造函數，透過依賴注入獲取 NorthwindContext
		public RegionsController(NorthwindContext context)
		{
			_context = context;
		}

		// 處理 GET 請求到 api/regions 的路由，用於獲取所有地區
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Region>>> GetRegions()
		{
			// 非同步地從數據庫中獲取所有地區並返回
			return await _context.Regions.ToListAsync();
		}

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
