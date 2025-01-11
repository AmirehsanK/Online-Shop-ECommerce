using Application.Services.Interfaces;
using Domain.Entities.Orders;
using Domain.Interface;
using Domain.ViewModel.Order;
using Domain.ViewModel.User;

namespace Application.Services.Impelementation;

public class OrderService(
    IOrderRepository orderRepository,
    IProductColorRepository colorRepository,
    IUserRepository userRepository) : IOrderService
{
    #region Add Product to Order

    public async Task AddProductToOrder(int productId, int userId, int? productColorId, int count = 1)
    {
        var openOrder = await orderRepository.GetUserLatestOpenOrder(userId);
        var existOrderDetail = await orderRepository.GetExistOrderDetail(productId, productColorId!.Value);
        if (existOrderDetail == null!)
        {
            var orderDetail = new OrderDetail
            {
                ProductId = productId,
                Count = count,
                OrderId = openOrder.Id,
                IsDeleted = false,
                CreateDate = DateTime.Now,
                ProductColorId = productColorId
            };

            if (productColorId.HasValue)
            {
                var colorPrice = await colorRepository.GetProductColorWithid(productColorId.Value);
                var color = colorPrice.Price;
                orderDetail.ColorPrice = color;
            }

            await orderRepository.AddOrderDetail(orderDetail);
        }
        else
        {
            existOrderDetail.Count = existOrderDetail.Count + 1;
            orderRepository.UpdateOrderDetail(existOrderDetail);
        }

        await orderRepository.Save();
    }

    #endregion

    #region Update Product Color Count

    public async Task MinuesColorCount(int productColorId)
    {
        var product = await colorRepository.GetProductColorWithid(productColorId);
        product.Count = product.Count - 1;
        colorRepository.UpdateProductColor(product);
        await colorRepository.SaveChangeAsync();
    }

    #endregion

    #region Get Basket Details

    public async Task<List<BasketDetailViewModel>> GetBasketDetail(int userId)
    {
        var details = await orderRepository.GetUserBasketDetail(userId);
        var details2 = new List<BasketDetailViewModel>();

        foreach (var item in details.OrderDetails)
        {
            var colorCode = await colorRepository.GetProductColorWithid(item.ProductColorId!.Value);
            var code = colorCode.Color.ColorCode;
            var name = colorCode.Color.Title;
            var newDetail = new BasketDetailViewModel
            {
                ColorCode = code,
                Title = item.Product.ProductName,
                ImageName = item.Product.ImageName,
                FinallPrice = (item.ColorPrice + item.Product.Price) * item.Count,
                OrderDetailId = item.Id,
                ColorName = name,
                ProductCount = item.Count,
                ProductId = item.ProductId
            };
            details2.Add(newDetail);
        }

        return details2;
    }

    #endregion

    #region Close Order

    public async Task CloseOrder(int userId)
    {
        var order = await orderRepository.GetUserLatestOpenOrder(userId);
        order.IsFinally = true;
        order.PaymentDate = DateTime.Now;
        orderRepository.UpdateOrder(order);
        await orderRepository.Save();
    }

    #endregion

    #region User Address for Order

    public async Task<GetUserAddressForOrderViewModel> GetUserAddressForOrder(int userId)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        var userAddress = new GetUserAddressForOrderViewModel
        {
            Address = user.Address!,
            FullName = user.FirstName + " " + user.LastName
        };
        return userAddress;
    }

    public async Task AddUserAddressForOrder(GetUserAddressForOrderViewModel model, int userid)
    {
        var user = await userRepository.GetUserByIdAsync(userid);
        user.Address = model.Address;
        userRepository.UpdateUser(user);
        await userRepository.SaveChangesAsync();
    }

    #endregion
}