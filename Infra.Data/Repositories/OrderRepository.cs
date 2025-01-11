
using Domain.Entities.Orders;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories
{
    public class OrderRepository
        (ApplicationDbContext context) : IOrderRepository
    {
        public async Task<Order> GetUserLatestOpenOrder(int userId)
        {
            var Order = await context.Orders.Include(u=> u.OrderDetails).FirstOrDefaultAsync(u => u.UserId == userId&&u.PaymentDate==null&&u.IsFinally==false);
            if (Order == null)
            {
                var neworder = new Order()
                {
                    CreateDate = DateTime.Now,
                    IsDeleted = false,
                    UserId = userId
                };
                await AddOrder(neworder);
                await Save();
                
                return neworder;
            }

            return Order;
        }

        public async Task<OrderDetail?> GetExistOrderDetail(int productId, int? ProductColorId,int orderid)
        {
            if (ProductColorId.HasValue)
            {
                return await context.OrderDetails.Where(u=>u.OrderId==orderid).FirstOrDefaultAsync(u =>
                    u.ProductId == productId && u.ProductColorId == ProductColorId.Value);
            }
            else
            {
                 return await context.OrderDetails.Where(u=>u.OrderId==orderid).FirstOrDefaultAsync(u =>
                    u.ProductId == productId && u.ProductColorId == null);
            }
           
        }

        public async Task AddOrder(Order order)
        {
            await context.Orders.AddAsync(order);
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public void UpdateOrder(Order order)
        {
            context.Orders.Update(order);
        }

        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            context.OrderDetails.Update(orderDetail);
        }

        public async Task AddOrderDetail(OrderDetail orderDetail)
        {
            await context.OrderDetails.AddAsync(orderDetail);
        }

        public async Task<Order> GetUserBasketDetail(int userId)
        {
            return await context.Orders.Where(u => u.UserId == userId && u.PaymentDate == null)
                .Include(u => u.OrderDetails).ThenInclude(u=> u.Product).FirstOrDefaultAsync();

        }
    }
}
