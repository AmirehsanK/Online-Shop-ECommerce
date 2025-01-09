using Domain.Entities.Orders;

namespace Domain.Interface;

public interface IOrderRepository
{

    #region Order Management

    Task<Order> GetUserLatestOpenOrder(int userId);
    Task<OrderDetail> GetExistOrderDetail(int productId, int ProductColorId);
    Task AddOrder(Order order);
    void UpdateOrder(Order order);
    void UpdateOrderDetail(OrderDetail orderDetail);
    Task AddOrderDetail(OrderDetail orderDetail);
    Task<Order> GetUserBasketDetail(int userId);

    #endregion

    #region Save Changes

    Task Save();

    #endregion

}