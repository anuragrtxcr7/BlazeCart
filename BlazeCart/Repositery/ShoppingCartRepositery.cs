//  using Microsoft.EntityFrameworkCore; --> This line is not needed in the Dapper-based repository
using BlazeCart.Data;
using BlazeCart.Repositery.IRepositery;
using System.Data;
using Dapper;

namespace BlazeCart.Repositery
{
    public class ShoppingCartRepositery : IShoppingCartRepositery
    {
        private readonly IDbConnection _db;
        public ShoppingCartRepositery(IDbConnection db)
        {
            _db = db;
        }

        //        private readonly ApplicationDbContext _db;
        //        public ShoppingCartRespositery(ApplicationDbContext db)
        //        {
        //            _db = db;
        //        }

        public async Task<IEnumerable<ShoppingCart>> GetAllAsync(string? userId)
        {
            var sql = @"SELECT sc.*, p.*  
                        FROM ShoppingCart sc 
                        INNER JOIN Products p ON sc.ProductId = p.Id 
                        WHERE sc.UserId = @UserId";

            var cartDict = new Dictionary<int, ShoppingCart>();

            var result = await _db.QueryAsync<ShoppingCart, Product, ShoppingCart>(
                sql,
                (cart, product) =>
                {
                    if (!cartDict.TryGetValue(cart.Id, out var existing))
                    {
                        existing = cart;
                        cartDict.Add(cart.Id, existing);
                    }
                    existing.Product = product;
                    return existing;
                },
                new { UserId = userId }
            );

            return cartDict.Values;
        }
        //        public async Task<IEnumerable<ShoppingCart>> GetAllAsync(string? userId)
        //        {
        //            return await _db.ShoppingCart.Where(u => u.UserId == userId).Include(u => u.Product).ToListAsync();
        //        }

        public async Task<bool> UpdateCartAsync(string userId, int productId, int updateBy)
        {
            if (string.IsNullOrEmpty(userId)) return false;

            var sqlSelect = @"SELECT * FROM ShoppingCart WHERE UserId = @UserId AND ProductId = @ProductId";
            var existing = await _db.QueryFirstOrDefaultAsync<ShoppingCart>(sqlSelect, new { UserId = userId, ProductId = productId });

            if (existing == null)
            {
                if (updateBy <= 0) return false;

                var insertSql = @"INSERT INTO ShoppingCart (UserId, ProductId, Count)
                                  VALUES (@UserId, @ProductId, @Count)";
                var rows = await _db.ExecuteAsync(insertSql, new { UserId = userId, ProductId = productId, Count = updateBy });
                return rows > 0;
            }
            else
            {
                int newCount = existing.Count + updateBy;
                if (newCount <= 0)
                {
                    var deleteSql = @"DELETE FROM ShoppingCart WHERE Id = @Id";
                    var rows = await _db.ExecuteAsync(deleteSql, new { Id = existing.Id });
                    return rows > 0;
                }
                else
                {
                    var updateSql = @"UPDATE ShoppingCart SET Count = @Count WHERE Id = @Id";
                    var rows = await _db.ExecuteAsync(updateSql, new { Count = newCount, Id = existing.Id });
                    return rows > 0;
                }
            }
        }
        //        public async Task<bool> UpdateCartAsync(string userId, int productId, int updateBy)
        //        {
        //            if (string.IsNullOrEmpty(userId))
        //            {
        //                return false;
        //            }

        //            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(u => u.UserId == userId && u.ProductId == productId);
        //            if (cart == null)
        //            {
        //                cart = new ShoppingCart
        //                {
        //                    UserId = userId,
        //                    ProductId = productId,
        //                    Count = updateBy
        //                };

        //                await _db.ShoppingCart.AddAsync(cart);
        //            }
        //            else
        //            {
        //                cart.Count += updateBy;
        //                if (cart.Count <= 0)
        //                {
        //                    _db.ShoppingCart.Remove(cart);
        //                }
        //            }
        //            return await _db.SaveChangesAsync() > 0;
        //        }
        //    }

        public async Task<bool> ClearCartAsync(string? userId)
        {
            var sql = @"DELETE FROM ShoppingCart WHERE UserId = @UserId";
            var rows = await _db.ExecuteAsync(sql, new { UserId = userId });
            return rows > 0;
        }
        //        public async Task<bool> ClearCartAsync(string? userId)
        //        {
        //            var cartItems = await _db.ShoppingCart.Where(u => u.UserId == userId).ToListAsync();
        //            _db.ShoppingCart.RemoveRange(cartItems);
        //            return await _db.SaveChangesAsync() > 0;
        //        }
    }
}
