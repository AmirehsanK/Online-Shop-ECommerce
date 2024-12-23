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
        Task<DiscountViewModel?> GetByIdAsync(int id);
        Task AddAsync(DiscountViewModel viewModel);
        Task UpdateAsync(int id, DiscountViewModel viewModel);
        Task DeleteAsync(int id);
    }
}
