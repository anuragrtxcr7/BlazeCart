using BlazeCart.Data;

namespace BlazeCart.Repositery.IRepositery
{
    public interface ICategoryRepositery
    {
        public Task <Category> CreateAsync(Category obj);
        public Task <Category> UpdateAsync(Category obj);
        public Task <bool> DeleteAsync(int id); // weather delete  is successful or not
        public Task<Category> GetAsync(int id);
        public Task<IEnumerable<Category>> GetAllAsync();
    }
}
