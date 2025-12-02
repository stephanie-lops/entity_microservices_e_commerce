using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.API.Data;
using Inventory.API.Models;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        public ProductsController(InventoryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] int quantity)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            product.Quantity = quantity;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveStock([FromBody] ReserveRequest request)
        {
            var product = await _context.Products.FindAsync(request.ProductId);

            if (product == null)
                return BadRequest("Produto não encontrado");

            if (product.Quantity < request.Quantity)
                return BadRequest("Estoque insuficiente");

            product.Quantity -= request.Quantity;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Estoque reservado com sucesso",
                productId = product.Id,
                newQuantity = product.Quantity
            });
        }

    }
}
