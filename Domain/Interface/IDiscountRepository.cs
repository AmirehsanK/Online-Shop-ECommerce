using Domain.Entities.Discount;
using Domain.ViewModel.User;

namespace Domain.Interface;

public interface IDiscountRepository
{
    #region Discount Retrieval

    Task<List<Discount>> GetAllAsync();
    Task<Discount?> GetByIdAsync(int id);
    Task<List<Discount>> GetActiveDiscounts();
    Task<Discount?> GetHighestDiscountForProductAsync(int productId);
    Task<List<UserDiscountViewModel>> GetUserGiftCodesAsync(int userId);

    #endregion

    #region Discount Management

    Task AddAsync(Discount discount);
    void Update(Discount discount);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();

    #endregion

    #region Discount Assignment

    Task<List<int>> GetUserDiscount(int discountId);
    Task<List<int>> GetProductDiscount(int discountId);
    Task AssignProductDiscountAsync(int productId, int discountId);
    Task AssignUserDiscountAsync(int userId, int discountId);
    Task RemoveUserDiscountAsync(int userId, int discountId);
    Task RemoveProductDiscountAsync(int productId, int discountId);

    #endregion
}