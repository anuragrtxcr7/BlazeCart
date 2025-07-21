using BlazeCart.Data;

namespace BlazeCart.Repositery.IRepositery
{
    public interface IOrderRepositery
    {
        public Task<Order> CreateAsync(Order order);
        public Task<Order> GetAsync(int id);
        public Task<IEnumerable<Order>> GetAllAsync(string? userId = null);
        public Task<Order> UpdateStatusAsync(int orderId, string status, string paymentIntentId);
    }
}
