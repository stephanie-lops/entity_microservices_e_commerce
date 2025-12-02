using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.API.Models;
using Sales.API.Dtos;


namespace Sales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly SalesDbContext _context;
        private readonly HttpClient _httpClient;

        public OrdersController(SalesDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient("inventory");
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            // Prepara o objeto que o Inventory espera
            var reserveRequest = new InventoryReserveRequest
            {
                ProductId = order.ProductId,
                Quantity = order.Quantity
            };

            // Chama o Inventory
            var response = await _httpClient.PostAsJsonAsync(
                "http://localhost:5212/api/products/reserve",
                reserveRequest
            );

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Produto não disponível em estoque");
            }

            // Salva o pedido
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

    }
}
