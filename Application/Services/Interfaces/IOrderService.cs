using Domain.Enums;
using Domain.ViewModel.Order;
using Domain.ViewModel.User;

namespace Application.Services.Interfaces;

public interface IOrderService
{
    Task<AddToBasketResult> AddProductToOrder(int productId, int userId, int? productColorId, int count = 1);
    Task CloseOrder(int userId, int transId);

    Task ChangeTransactionStatus(int transid);

    #region Order Management

    Task<List<BasketDetailViewModel>> GetBasketDetail(int userId);
    Task<GetUserAddressForOrderViewModel> GetUserAddressForOrder(int userId);
    Task AddUserAddressForOrder(GetUserAddressForOrderViewModel model, int userid);

    Task MinuesColorCount(int productColorId);

    #endregion
}