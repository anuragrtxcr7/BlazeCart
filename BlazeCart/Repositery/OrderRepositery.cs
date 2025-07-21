using Microsoft.EntityFrameworkCore;
using BlazeCart.Data;
using BlazeCart.Repositery.IRepositery;

namespace BlazeCart.Repositery
{
    public class OrderRepositery : IOrderRepositery
    {
        private readonly ApplicationDbContext _db;

        public OrderRepositery(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Order> CreateAsync(Order orderHeader)
        {
            orderHeader.OrderDate = DateTime.Now;
            await _db.Orders.AddAsync(orderHeader);
            await _db.SaveChangesAsync();
            return orderHeader;
        }

        public async Task<IEnumerable<Order>> GetAllAsync(string? userId = null)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                return await _db.Orders.Where(u => u.UserId == userId).ToListAsync();
            }
            return await _db.Orders.ToListAsync();
        }

        public async Task<Order> GetAsync(int id)
        {
            return await _db.Orders.Include(u => u.OrderItems).FirstOrDefaultAsync(u => u.Id == id);
        }


        public async Task<Order> UpdateStatusAsync(int orderId, string status, string paymentIntentId)
        {
            var orderHeader = _db.Orders.FirstOrDefault(u => u.Id == orderId);
            if (orderHeader != null)
            {
                orderHeader.Status = status;
                await _db.SaveChangesAsync();
            }
            return orderHeader;
        }
    }
    
}
