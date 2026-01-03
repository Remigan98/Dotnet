using YumBlazor.Data;

namespace YumBlazor.Utility
{
    public static class StaticDetails
    {
        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";

        public static string StatusPending = "Pending";
        public static string StatusApproved = "Approved";
        public static string StatusReadyForPickup = "Ready for Pickup";
        public static string StatusCompleted = "Completed";
        public static string StatusCancelled = "Cancelled";

        public static List<OrderDetail> ConvertShoppingCartListToOrderDetails(List<ShoppingCart> shoppingCarts)
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            foreach (ShoppingCart cart in shoppingCarts)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = cart.ProductId,
                    Product = cart.Product,
                    Count = cart.Count,
                    ProductName = cart.Product.Name,
                    Price = cart.Product.Price
                };

                orderDetails.Add(orderDetail);
            }

            return orderDetails;
        }
    }
}
