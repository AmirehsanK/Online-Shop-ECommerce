using Domain.Entities.Discount;
using Domain.ViewModel.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<List<DiscountViewModel>> GetAllAsync();
        Task<List<DiscountListAdminViewModel>> GetAllForAdminAsync();
        Task<DiscountViewModel?> GetByIdAsync(int id);
        Task AddAsync(DiscountViewModel viewModel);
        Task UpdateAsync(int id, DiscountEditViewModel viewModel);
        Task DeleteAsync(int id);

        Task<List<int>> GetUserDiscount(int discountId);
        Task<List<int>> GetProductDiscount(int discountId);
        Task AssignProductDiscountAsync(List<int> productId, int discountId);
        Task AssignUserDiscountAsync(List<int> userId, int discountId);
    }
}
