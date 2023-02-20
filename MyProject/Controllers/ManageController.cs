using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageController : ControllerBase
    {

        private NorthwindContext _context;

        public ManageController(NorthwindContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> ProductList()
        {
            return await _context.Products.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductItem(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProductItem(Product productItem)
        {
            _context.Products.Add(productItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProductItem",
                     new { id = productItem.ProductId },productItem);

        }
    }
}
