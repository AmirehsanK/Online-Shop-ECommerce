using Domain.Entities.Orders;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class OrderRepository(ApplicationDbContext context) : IOrderRepository
{
    #region Save Changes

    public async Task Save()
    {
        await context.SaveChangesAsync();
    }

    #endregion

    #region Order Methods

    public async Task<Order> GetUserLatestOpenOrder(int userId)
    {
        var order = await context.Orders
            .Include(u => u.OrderDetails)
            .FirstOrDefaultAsync(u => u.UserId == userId&&u.IsFinally==false);

        if (order != null) return order;

        var newOrder = new Order
        {
            CreateDate = DateTime.Now,
            IsDeleted = false,
            UserId = userId
        };
        await AddOrder(newOrder);
        await Save();

        return newOrder;
    }

    public async Task<OrderDetail?> GetExistOrderDetail(int productId, int? ProductColorId, int orderId)
    {
        if (ProductColorId!=null)
        {
            return await context.OrderDetails.Where(u => u.OrderId == orderId)
                .FirstOrDefaultAsync(u => u.ProductId == productId && u.ProductColorId == ProductColorId);
        }
        else
        {
            return await context.OrderDetails.Where(u => u.OrderId == orderId)
                .FirstOrDefaultAsync(u => u.ProductId == productId);
        }
    }

    public async Task AddOrder(Order order)
    {
        await context.Orders.AddAsync(order);
    }

    public void UpdateOrder(Order order)
    {
        context.Orders.Update(order);
    }

    public async Task<Order> GetUserBasketDetail(int userId)
    {
        return await context.Orders
            .Where(u => u.UserId == userId && u.PaymentDate == null)
            .Include(u => u.OrderDetails)
            .ThenInclude(u => u.Product)
            .FirstOrDefaultAsync();
    }

    #endregion

    #region Order Detail Methods


    public async Task AddOrderDetail(OrderDetail orderDetail)
    {
        await context.OrderDetails.AddAsync(orderDetail);
    }

    public void UpdateOrderDetail(OrderDetail orderDetail)
    {
        context.OrderDetails.Update(orderDetail);
    }

    #endregion
}