using Domain.ViewModel.Discount;

namespace Application.Services.Interfaces;

public interface IDiscountService
{

    #region Discount Retrieval

    Task<List<DiscountViewModel>> GetAllAsync();
    Task<List<DiscountListAdminViewModel>> GetAllForAdminAsync();
    Task<DiscountViewModel?> GetByIdAsync(int id);

    #endregion

    #region Discount Management

    Task AddAsync(DiscountViewModel viewModel);
    Task UpdateAsync(int id, DiscountEditViewModel viewModel);
    Task DeleteAsync(int id);

    #endregion

    #region Discount Assignment

    Task<List<int>> GetUserDiscount(int discountId);
    Task<List<int>> GetProductDiscount(int discountId);
    Task AssignProductDiscountAsync(List<int> productId, int discountId);
    Task AssignUserDiscountAsync(List<int> userId, int discountId);

    #endregion

}