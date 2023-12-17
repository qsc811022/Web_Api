using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using NorthwindWebAPI.Models;

namespace NorthwindWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegionsController : ControllerBase
	{
		private readonly NorthwindContext _context;

		public RegionsController(NorthwindContext context)
		{
			_context = context;
		}

		// GET: api/regions
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Region>>> GetRegions()
		{
			return await _context.Regions.ToListAsync();
		}

		// GET: api/regions/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Region>> GetRegion(int id)
		{
			var region = await _context.Regions.FindAsync(id);

			if (region == null)
			{
				return NotFound();
			}

			return region;
		}

		// POST: api/regions
		[HttpPost]
		public async Task<ActionResult<Region>> PostRegion(Region region)
		{
			_context.Regions.Add(region);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetRegion), new { id = region.RegionId }, region);
		}

		// PUT: api/regions/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutRegion(int id, Region region)
		{
			if (id != region.RegionId)
			{
				return BadRequest();
			}

			_context.Entry(region).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// DELETE: api/regions/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteRegion(int id)
		{
			var region = await _context.Regions.FindAsync(id);
			if (region == null)
			{
				return NotFound();
			}

			_context.Regions.Remove(region);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}
