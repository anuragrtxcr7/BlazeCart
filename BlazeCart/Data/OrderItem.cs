using System.ComponentModel.DataAnnotations.Schema;

namespace BlazeCart.Data
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int Count { get; set; }

        public double Price { get; set; }

        public string ProductName { get; set; }
    }
}
