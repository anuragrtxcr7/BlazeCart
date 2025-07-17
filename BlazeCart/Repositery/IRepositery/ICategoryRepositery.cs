using BlazeCart.Data;

namespace BlazeCart.Repositery.IRepositery
{
    public interface ICategoryRepositery
    {
        public Category Create(Category obj);
        public Category Update(Category obj);
        public bool Delete(int id); // weather delete  is successful or not
        public Category Get(int id);
        public IEnumerable<Category> GetAll();
    }
}
