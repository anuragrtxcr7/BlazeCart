using BlazeCart.Data;
using BlazeCart.Repositery.IRepositery;

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

        public Category Create(Category obj)
        {
            _db.Categories.Add(obj);
            _db.SaveChanges();
            return obj;
        }

        public bool Delete(int id)
        {
            var obj = _db.Categories.FirstOrDefault(c => c.Id == id);
            if(obj == null)
            {
                return false; // Category not found
            }
            else {                 
                _db.Categories.Remove(obj);
                return _db.SaveChanges() > 0; // Save changes to the database and returns number of affected rows
            }
        }

        public Category Get(int id)
        {
            var obj = _db.Categories.FirstOrDefault(c => c.Id == id);
            if(obj == null)
            {
                return new Category(); // Category not found
            }
            else
            {
                return obj; // Return the found category
            }
        }

        public IEnumerable<Category> GetAll()
        {
            return _db.Categories.ToList(); // Return all categories as a list
        }

        public Category Update(Category obj)
        {
            var objFromDb = _db.Categories.FirstOrDefault(c => c.Id == obj.Id);
            if (objFromDb == null)
            {
                return obj; // If the category is not found, return the original object
            }
            else
            {
                objFromDb.Name = obj.Name; // Update the name or other properties as needed
                _db.SaveChanges();
                return objFromDb; // Return the updated category
            }
        }
    }
}
