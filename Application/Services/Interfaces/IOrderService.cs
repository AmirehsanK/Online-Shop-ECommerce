

using Domain.ViewModel.Order;
using Domain.ViewModel.User;

namespace Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task AddProductToOrder(int productId, int userId,int? productColorId,  int count = 1);

        Task MinuesColorCount(int productColorId);


        Task<List<BasketDetailViewModel>> GetBasketDetail(int userId);
        Task<GetUserAddressForOrderViewModel> GetUserAddressForOrder(int userId);

        Task AddUserAddressForOrder(GetUserAddressForOrderViewModel model,int userid);

    }
}
