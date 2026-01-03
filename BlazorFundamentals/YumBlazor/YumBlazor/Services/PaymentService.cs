using Microsoft.AspNetCore.Components;
using Stripe;
using Stripe.Checkout;
using YumBlazor.Data;
using YumBlazor.Repository.Interfaces;

namespace YumBlazor.Services
{
    public class PaymentService
    {
        readonly NavigationManager navigationManager;
        readonly IOrderRepository orderRepository;

        public PaymentService(NavigationManager navigationManager, IOrderRepository orderRepository)
        {
            this.navigationManager = navigationManager;
            this.orderRepository = orderRepository;
        }

        public Session CreateCheckoutSession(OrderHeader orderHeader)
        {
            var lineItems = orderHeader.OrderDetails
                .Select(order => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmountDecimal = (decimal?)order.Price * 100,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = order.ProductName,
                        },
                    },
                    Quantity = order.Count,
                }).ToList();

            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{navigationManager.BaseUri}order/confirmation/{{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{navigationManager.BaseUri}cart",
                LineItems = lineItems,
                Mode = "payment",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session;
        }

        public async Task<OrderHeader> CheckPaymentStatusAndUpdateOrder(string sessionId)
        {
            SessionService service = new SessionService();
            Session session = service.Get(sessionId);
            OrderHeader orderHeader = await orderRepository.GetOrderBySessionIdAsync(session.Id);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                orderHeader = await orderRepository.UpdateOrderStatusAsync(orderHeader.Id, "Approved", session.PaymentIntentId);
            }

            return orderHeader;
        }
    }
}
