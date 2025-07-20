﻿using BlazeCart.Data;

namespace BlazeCart.Repositery.IRepositery
{
    public interface IShoppingCartRepositery
    {
        public Task<bool> UpdateCartAsync(string userId, int product, int updateBy);
        public Task<IEnumerable<ShoppingCart>> GetAllAsync(string? userId);
        public Task<bool> ClearCartAsync(string? userId);
    }
}
