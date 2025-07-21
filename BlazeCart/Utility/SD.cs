using BlazeCart.Data;

namespace BlazeCart.Utility
{
    public static class SD // sd --> static data
    {
        public static string Role_Admin = "Admin";
        public static string Role_Customer = "Customer";

        public static string StatusPending = "Pending";
        public static string StatusApproved = "Approved";
        public static string StatusReadyForPickUp = "ReadyForPickUp";
        public static string StatusCompleted = "Completed";
        public static string StatusCancelled = "Cancelled";

        public static List<OrderItem> ConvertShoppingCartListToOrderItem(List<ShoppingCart> shoppingCarts)
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            foreach (var cart in shoppingCarts)
            {
                OrderItem orderItem = new OrderItem
                {
                    ProductId = cart.ProductId,
                    Count = cart.Count,
                    Price = Convert.ToDouble(cart.Product.Price),
                    ProductName = cart.Product.Name
                };
                orderItems.Add(orderItem);
            }

            return orderItems;
        }
    }
}
