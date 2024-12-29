using Domain.Entities.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IDiscountRepository
    {
        Task<List<Discount>> GetAllAsync();
        Task<Discount?> GetByIdAsync(int id);
        Task<List<Discount>> GetActiveDiscounts();
        Task<Discount?> GetHighestDiscountForProductAsync(int productId);
        Task AddAsync(Discount discount);
        Task UpdateAsync(Discount discount);
        Task DeleteAsync(int id);

        Task<List<int>> GetUserDiscount(int discountId);
        Task<List<int>> GetProductDiscount(int discountId);
        Task AssignProductDiscountAsync(int productId, int discountId);
        Task AssignUserDiscountAsync(int userId, int discountId);
        Task RemoveUserDiscountAsync(int userId, int discountId);
        Task RemoveProductDiscountAsync(int productId, int discountId);
        Task SaveChangesAsync();
    }
}
