using BlazeCart.Data;

namespace BlazeCart.Repositery.IRepositery
{
    public interface IProductRepositery
    {
        public Task <Product> CreateAsync(Product obj);
        public Task <Product> UpdateAsync(Product obj);
        public Task <bool> DeleteAsync(int id); // weather delete  is successful or not
        public Task<Product> GetAsync(int id);
        public Task<IEnumerable<Product>> GetAllAsync();
    }
}
