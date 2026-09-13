using Application.Services.Interfaces;
using Domain.Entities.Orders;
using Domain.Enums;
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

    public async Task<AddToBasketResult> AddProductToOrder(int productId, int userId, int? productColorId,
        int count = 1)
    {
        int colorPrice = 0;
        if (productColorId.HasValue)
        {
            var color = await colorRepository.GetProductColorWithid(productColorId.Value);
            if (color == null || color.ProductId != productId)
                return AddToBasketResult.Failed;

            // Take the unit first. Stock used to be decremented after the fact with no check,
            // so it went negative and sold-out variants could still be bought.
            if (!await colorRepository.TryReserveOneAsync(productColorId.Value))
                return AddToBasketResult.OutOfStock;

            colorPrice = color.Price;
        }

        var openOrder = await orderRepository.GetUserLatestOpenOrder(userId);
        var existOrderDetail = await orderRepository.GetExistOrderDetail(productId, productColorId, openOrder.Id);
        if (existOrderDetail == null)
        {
            await orderRepository.AddOrderDetail(new OrderDetail
            {
                ProductId = productId,
                Count = count,
                OrderId = openOrder.Id,
                IsDeleted = false,
                CreateDate = DateTime.Now,
                ProductColorId = productColorId,
                ColorPrice = colorPrice
            });
        }
        else
        {
            existOrderDetail.Count += 1;
            orderRepository.UpdateOrderDetail(existOrderDetail);
        }

        await orderRepository.Save();
        return AddToBasketResult.Success;
    }

    #endregion

    public async Task<List<BasketDetailViewModel>> GetBasketDetail(int userId)
    {
        var basket = await orderRepository.GetUserBasketDetail(userId);
        if (basket == null) return [];

        var details = new List<BasketDetailViewModel>();
        foreach (var item in basket.OrderDetails.Where(d => !d.IsDeleted))
        {
            // Looked up per row: these used to be declared outside the loop, so an item
            // without a colour showed the colour of the item before it.
            var productColor = item.ProductColorId.HasValue
                ? await colorRepository.GetProductColorWithid(item.ProductColorId)
                : null;

            details.Add(new BasketDetailViewModel
            {
                ColorCode = productColor?.Color.ColorCode,
                ColorName = productColor?.Color.Title,
                Title = item.Product.ProductName,
                ImageName = item.Product.ImageName,
                FinallPrice = (item.ColorPrice + item.Product.Price) * item.Count,
                OrderDetailId = item.Id,
                ProductCount = item.Count,
                ProductId = item.ProductId
            });
        }

        return details;
    }

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

    #endregion

    public async Task AddUserAddressForOrder(GetUserAddressForOrderViewModel model, int userid)
    {
        var user = await userRepository.GetUserByIdAsync(userid);
        user.Address = model.Address;
        userRepository.UpdateUser(user);
        await userRepository.SaveChangesAsync();
    }
}
