using BlazeCart.Data;
using BlazeCart.Repositery.IRepositery;
using Microsoft.EntityFrameworkCore;

namespace BlazeCart.Repositery
{
    public class ProductRepositery : IProductRepositery
    {
        private readonly ApplicationDbContext _db;

        // Constructor to initialize the ApplicationDbContext

        public ProductRepositery(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Product> CreateAsync(Product obj)
        {
            await _db.Products.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var obj = await _db.Products.FirstOrDefaultAsync(c => c.Id == id);
            if(obj == null)
            {
                return false; // Product not found
            }
            else {                 
                _db.Products.Remove(obj);
                return (await _db.SaveChangesAsync()) > 0; // Save changes to the database and returns number of affected rows
            }
        }

        public async Task<Product> GetAsync(int id)
        {
            var obj = await _db.Products.FirstOrDefaultAsync(c => c.Id == id);
            if(obj == null)
            {
                return new Product(); // Product not found
            }
            else
            {
                return obj; // Return the found product
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Products.ToListAsync(); // Return all products as a list
        }

        public async Task<Product> UpdateAsync(Product obj)
        {
            var objFromDb = await _db.Products.FirstOrDefaultAsync(c => c.Id == obj.Id);
            if (objFromDb == null)
            {
                return obj; // If the product is not found, return the original object
            }
            else
            {
                objFromDb.Name = obj.Name; // Update the name or other properties as needed
                objFromDb.Price = obj.Price; // Assuming Product has a Price property
                objFromDb.Description = obj.Description; // Assuming Product has a Description property
                objFromDb.CategoryId = obj.CategoryId; // Assuming Product has a CategoryId property
                objFromDb.ImageUrl = obj.ImageUrl; // Assuming Product has an ImageUrl property
                await _db.SaveChangesAsync();
                return objFromDb; // Return the updated product
            }
        }
    }
}
