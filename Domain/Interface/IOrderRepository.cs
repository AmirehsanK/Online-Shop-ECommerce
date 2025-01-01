

using Domain.Entities.Orders;

namespace Domain.Interface
{
    public interface IOrderRepository
    {
        Task<Order> GetUserLatestOpenOrder(int userId);
        
        Task<OrderDetail> GetExistOrderDetail(int productId, int ProductColorId);
        Task AddOrder(Order order);

        Task Save();

        void UpdateOrder(Order order);
        void UpdateOrderDetail(OrderDetail orderDetail);

        Task AddOrderDetail(OrderDetail orderDetail);

        Task<Order> GetUserBasketDetail(int userId);

    }
}
