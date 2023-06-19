using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PCBuilder.Repository.Model;
using PCBuilder.Services.DTO;
using PCBuilder.Services.Service;

namespace PCBuilder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var response = await _orderServices.GetOrderById(orderId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var response = await _orderServices.GetAllOrders();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDTO)
        {
            var order = new Order
            {
                // Map orderDTO properties to Order entity properties
                // Example: order.Property = orderDTO.Property;
            };

            var response = await _orderServices.CreateOrder(order);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] OrderDTO orderDTO)
        {
            var existingOrderResponse = await _orderServices.GetOrderById(orderId);

            if (!existingOrderResponse.Success)
            {
                return NotFound(existingOrderResponse);
            }

            var existingOrder = existingOrderResponse.Data;

            // Update existingOrder properties using orderDTO properties
            // Example: existingOrder.Property = orderDTO.Property;

            var response = await _orderServices.UpdateOrder(existingOrder);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var response = await _orderServices.DeleteOrder(orderId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
