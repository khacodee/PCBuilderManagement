using PCBuilder.Repository.Model;
using PCBuilder.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBuilder.Services.Service
{
    public interface IOrderServices
    {
        Task<ServiceResponse<Order>> GetOrderById(int orderId);
        Task<ServiceResponse<List<Order>>> GetAllOrders();
        Task<ServiceResponse<Order>> CreateOrder(Order order);
        Task<ServiceResponse<Order>> UpdateOrder(Order order);
        Task<ServiceResponse<bool>> DeleteOrder(int orderId);
    }
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepository;

        public OrderServices(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<ServiceResponse<Order>> GetOrderById(int orderId)
        {
            var response = new ServiceResponse<Order>();
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                response.Success = false;
                response.Message = "Order not found.";
            }
            else
            {
                response.Success = true;
                response.Message = "Order retrieved successfully.";
                response.Data = order;
            }
            return response;
        }

        public async Task<ServiceResponse<List<Order>>> GetAllOrders()
        {
            var response = new ServiceResponse<List<Order>>();
            var orders = await _orderRepository.GetAllOrdersAsync();
            response.Success = true;
            response.Message = "Orders retrieved successfully.";
            response.Data = orders;
            return response;
        }

        public async Task<ServiceResponse<Order>> CreateOrder(Order order)
        {
            var response = new ServiceResponse<Order>();
            var createdOrder = await _orderRepository.CreateOrderAsync(order);
            response.Success = true;
            response.Message = "Order created successfully.";
            response.Data = createdOrder;
            return response;
        }

        public async Task<ServiceResponse<Order>> UpdateOrder(Order order)
        {
            var response = new ServiceResponse<Order>();
            var existingOrder = await _orderRepository.GetOrderByIdAsync(order.Id);
            if (existingOrder == null)
            {
                response.Success = false;
                response.Message = "Order not found.";
            }
            else
            {
                var updatedOrder = await _orderRepository.UpdateOrderAsync(order);
                response.Success = true;
                response.Message = "Order updated successfully.";
                response.Data = updatedOrder;
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteOrder(int orderId)
        {
            var response = new ServiceResponse<bool>();
            var result = await _orderRepository.DeleteOrderAsync(orderId);
            if (result)
            {
                response.Success = true;
                response.Message = "Order deleted successfully.";
                response.Data = true;
            }
            else
            {
                response.Success = false;
                response.Message = "Order not found.";
                response.Data = false;
            }
            return response;
        }
    }
}
