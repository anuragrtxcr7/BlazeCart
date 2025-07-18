using BlazeCart.Data;
using BlazeCart.Repositery.IRepositery;
using Microsoft.EntityFrameworkCore;

namespace BlazeCart.Repositery
{
    public class CategoryRepositery : ICategoryRepositery
    {
        private readonly ApplicationDbContext _db;

        // Constructor to initialize the ApplicationDbContext

        public CategoryRepositery(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Category> CreateAsync(Category obj)
        {
            await _db.Categories.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var obj = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if(obj == null)
            {
                return false; // Category not found
            }
            else {                 
                _db.Categories.Remove(obj);
                return (await _db.SaveChangesAsync()) > 0; // Save changes to the database and returns number of affected rows
            }
        }

        public async Task<Category> GetAsync(int id)
        {
            var obj = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if(obj == null)
            {
                return new Category(); // Category not found
            }
            else
            {
                return obj; // Return the found category
            }
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _db.Categories.ToListAsync(); // Return all categories as a list
        }

        public async Task<Category> UpdateAsync(Category obj)
        {
            var objFromDb = await _db.Categories.FirstOrDefaultAsync(c => c.Id == obj.Id);
            if (objFromDb == null)
            {
                return obj; // If the category is not found, return the original object
            }
            else
            {
                objFromDb.Name = obj.Name; // Update the name or other properties as needed
                await _db.SaveChangesAsync();
                return objFromDb; // Return the updated category
            }
        }
    }
}
