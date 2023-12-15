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

		// GET: api/Products
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			return await _context.Products.ToListAsync();
		}

	}
}
