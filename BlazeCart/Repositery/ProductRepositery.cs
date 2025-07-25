using BlazeCart.Data;
using BlazeCart.Repositery.IRepositery;
using System.Data;
using Dapper;

namespace BlazeCart.Repositery
{
    public class ProductRepositery : IProductRepositery
    {
        private readonly IDbConnection _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductRepositery(IDbConnection db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Product> CreateAsync(Product obj)
        {
            var sql = @"INSERT INTO Products (Name, Price, Description, CategoryId, ImageUrl)
                        VALUES (@Name, @Price, @Description, @CategoryId, @ImageUrl);
                        SELECT CAST(SCOPE_IDENTITY() as int);";

            obj.Id = await _db.ExecuteScalarAsync<int>(sql, obj);
            return obj;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var getImageSql = "SELECT ImageUrl FROM Products WHERE Id = @Id";
            var imageUrl = await _db.ExecuteScalarAsync<string>(getImageSql, new { Id = id });

            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
                if (File.Exists(imagePath))
                    File.Delete(imagePath);
            }

            var deleteSql = "DELETE FROM Products WHERE Id = @Id";
            var affected = await _db.ExecuteAsync(deleteSql, new { Id = id });
            return affected > 0;
        }

        public async Task<Product> GetAsync(int id)
        {
            var sql = "SELECT * FROM Products WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id }) ?? new Product();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var sql = @"SELECT p.*, c.Id, c.Name 
                        FROM Products p
                        INNER JOIN Categories c ON p.CategoryId = c.Id";

            var productDict = new Dictionary<int, Product>();
             
            var result = await _db.QueryAsync<Product, Category, Product>(
                sql,
                (product, category) =>
                {
                    if (!productDict.TryGetValue(product.Id, out var existing))
                    {
                        existing = product;
                        productDict.Add(product.Id, existing);
                    }
                    existing.Category = category;
                    return existing;
                });

            return productDict.Values;
        }

        public async Task<Product> UpdateAsync(Product obj)
        {
            var sql = @"UPDATE Products
                        SET Name = @Name,
                            Price = @Price,
                            Description = @Description,
                            CategoryId = @CategoryId,
                            ImageUrl = @ImageUrl
                        WHERE Id = @Id";

            await _db.ExecuteAsync(sql, obj);
            return obj;
        }
    }
}

// With entity framework, the code would look like this:

//using BlazeCart.Data;
//using BlazeCart.Repositery.IRepositery;
//using Microsoft.EntityFrameworkCore;

//namespace BlazeCart.Repositery
//{
//    public class ProductRepositery : IProductRepositery
//    {
//        private readonly ApplicationDbContext _db;
//        private readonly IWebHostEnvironment _webHostEnvironment;

//        // Constructor to initialize the ApplicationDbContext

//        public ProductRepositery(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
//        {
//            _db = db;
//            _webHostEnvironment = webHostEnvironment;
//        }

//        public async Task<Product> CreateAsync(Product obj)
//        {
//            await _db.Products.AddAsync(obj);
//            await _db.SaveChangesAsync();
//            return obj;
//        }

//        public async Task<bool> DeleteAsync(int id)
//        {
//            var obj = await _db.Products.FirstOrDefaultAsync(c => c.Id == id);
//            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('/'));
//            if (File.Exists(imagePath))
//            {
//                File.Delete(imagePath);
//            }
//            if (obj != null)
//            {
//                _db.Products.Remove(obj);
//                return (await _db.SaveChangesAsync()) > 0; // Save changes to the database and returns number of affected rows
//            }
//            return false; // If the product was not found, return false
//        }

//        public async Task<Product> GetAsync(int id)
//        {
//            var obj = await _db.Products.FirstOrDefaultAsync(c => c.Id == id);
//            if(obj == null)
//            {
//                return new Product(); // Product not found
//            }
//            else
//            {
//                return obj; // Return the found product
//            }
//        }

//        public async Task<IEnumerable<Product>> GetAllAsync()
//        {
//            return await _db.Products.Include(u => u.Category).ToListAsync(); // Return all products as a list
//        }

//        public async Task<Product> UpdateAsync(Product obj)
//        {
//            var objFromDb = await _db.Products.FirstOrDefaultAsync(c => c.Id == obj.Id);
//            if (objFromDb == null)
//            {
//                return obj; // If the product is not found, return the original object
//            }
//            else
//            {
//                objFromDb.Name = obj.Name; // Update the name or other properties as needed
//                objFromDb.Price = obj.Price; // Assuming Product has a Price property
//                objFromDb.Description = obj.Description; // Assuming Product has a Description property
//                objFromDb.CategoryId = obj.CategoryId; // Assuming Product has a CategoryId property
//                objFromDb.ImageUrl = obj.ImageUrl; // Assuming Product has an ImageUrl property
//                await _db.SaveChangesAsync();
//                return objFromDb; // Return the updated product
//            }
//        }
//    }
//}