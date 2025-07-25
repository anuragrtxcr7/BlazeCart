//using Microsoft.EntityFrameworkCore;
using BlazeCart.Data;
using BlazeCart.Repositery.IRepositery;
using System.Data;
using Dapper;

namespace BlazeCart.Repositery
{
    public class OrderRepositery : IOrderRepositery
    {
        private readonly IDbConnection _db;

        public OrderRepositery(IDbConnection db)
        {
            _db = db;
        }


        public async Task<Order> CreateAsync(Order orderHeader)
        {
            var sql = @"INSERT INTO Orders (UserId, OrderTotal, OrderDate, Status, Name, PhoneNumber)
                        VALUES (@UserId, @OrderTotal, @OrderDate, @Status, @Name, @PhoneNumber);
                        SELECT CAST(SCOPE_IDENTITY() as int);";
            // SELECT CAST(SCOPE_IDENTITY() as int) statement is used to get the last inserted ID in SQL Server.

            orderHeader.OrderDate = DateTime.Now;
            orderHeader.Id = await _db.ExecuteScalarAsync<int>(sql, orderHeader);

            // Insert OrderItems if present
            foreach (var item in orderHeader.OrderItems)
            {
                item.OrderId = orderHeader.Id;

                var itemSql = @"INSERT INTO OrderItems (OrderId, ProductId, Count, Price, ProductName)
                                VALUES (@OrderId, @ProductId, @Count, @Price, @ProductName)";
                await _db.ExecuteAsync(itemSql, item);
            }

            return orderHeader;
        }

        public async Task<IEnumerable<Order>> GetAllAsync(string? userId = null)
        {
            var sql = string.IsNullOrEmpty(userId)
                ? "SELECT * FROM Orders"
                : "SELECT * FROM Orders WHERE UserId = @UserId";

            return await _db.QueryAsync<Order>(sql, new { UserId = userId });
        }

        public async Task<Order?> GetAsync(int id)
        {
            var orderSql = "SELECT * FROM Orders WHERE Id = @Id";
            var order = await _db.QueryFirstOrDefaultAsync<Order>(orderSql, new { Id = id });
            if (order == null) return null;

            var itemSql = @"SELECT oi.*, p.*  
                            FROM OrderItems oi
                            INNER JOIN Products p ON oi.ProductId = p.Id
                            WHERE oi.OrderId = @OrderId";
            // @ is added at the start @"Select" so that we can use multi-line strings in C#.
            // otherwise, we would have to use \n for new lines or + to concatenate strings.

            var orderItems = new List<OrderItem>();

            var lookup = new Dictionary<int, OrderItem>();

            await _db.QueryAsync<OrderItem, Product, OrderItem>(
                itemSql,
                (item, product) =>
                {
                    if (!lookup.TryGetValue(item.Id, out var existing))
                    {
                        existing = item;
                        lookup[item.Id] = existing;
                    }
                    existing.Product = product;
                    return existing;
                },
                new { OrderId = id });

            order.OrderItems = lookup.Values.ToList();
            return order;
        }


        public async Task<Order> UpdateStatusAsync(int orderId, string status, string paymentIntentId)
        {
            var sql = "UPDATE Orders SET Status = @Status WHERE Id = @OrderId";
            await _db.ExecuteAsync(sql, new { Status = status, OrderId = orderId });

            var updatedOrder = await GetAsync(orderId);
            return updatedOrder!;
        }
    }
}

//        private readonly ApplicationDbContext _db;
//        public OrderRepositery(ApplicationDbContext db)
//        {
//            _db = db;
//        }

//        public async Task<Order> CreateAsync(Order order)
//        {
//            order.OrderDate = DateTime.Now;
//            await _db.Orders.AddAsync(order);
//            await _db.SaveChangesAsync();
//            return order;
//        }

//        public async Task<IEnumerable<Order>> GetAllAsync(string? userId = null)
//        {
//            if (!string.IsNullOrEmpty(userId))
//            {
//                return await _db.Orders.Where(u => u.UserId == userId).ToListAsync();
//            }
//            return await _db.Orders.ToListAsync();
//        }

//        public async Task<Order> GetAsync(int id)
//        {
//            return await _db.Orders.Include(u => u.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(u => u.Id == id);
//        }

//        public async Task<Order> UpdateStatusAsync(int orderId, string status, string paymentIntentId)
//        {
//            var order = _db.Orders.FirstOrDefault(u => u.Id == orderId);
//            if (order != null)
//            {
//                order.Status = status;
//                await _db.SaveChangesAsync();
//            }
//            return order;
//        }