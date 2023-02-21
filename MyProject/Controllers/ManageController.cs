using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Models;
using System.Collections.Generic;
using System.Linq;
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
            //當資料新增時，同時利用 GetProductItem 這個Action(利用id去查詢) 看生成了什麼資料
            return CreatedAtAction("GetProductItem",
                     new { id = productItem.ProductId },productItem);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductItem(int id, Product productItem)
        {
            _context.Entry(productItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProductItem(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.OrderDetails.Where(x => x.ProductId == id).ToListAsync();

            _context.OrderDetails.RemoveRange(orderDetails);
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
