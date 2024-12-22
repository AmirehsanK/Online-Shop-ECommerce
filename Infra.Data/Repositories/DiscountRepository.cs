using Domain.Entities.Account;
using Domain.Entities.Discount;
using Domain.Interface;
using Infra.Data.Context;
using Infra.Data.Migrations;
using Microsoft.EntityFrameworkCore;
using Discount = Domain.Entities.Discount.Discount;


namespace Infra.Data.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly ApplicationDbContext _context;

        public DiscountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Discount>> GetAllAsync()
        {
            return await _context.Discounts.Where(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<Discount?> GetByIdAsync(int id)
        {
            return await _context.Discounts.FindAsync(id);
        }

        public async Task AddAsync(Discount discount)
        {
            await _context.Discounts.AddAsync(discount);
        }

        public async Task UpdateAsync(Discount discount)
        {
            _context.Discounts.Update(discount);
        }

        public async Task DeleteAsync(int id)
        {
            var discount = await GetByIdAsync(id);
            if (discount != null)
            {
                _context.Discounts.Remove(discount);
            }
        }
        public async Task<List<int>> GetUserDiscount(int discountId)
        {
            return await _context.UserDiscounts.Where(ud => ud.DiscountId == discountId).Select(ud => ud.UserId).ToListAsync();
        }
        public async Task<List<int>> GetProductDiscount(int discountId)
        {
            return await _context.ProductDiscounts.Where(ud => ud.DiscountId == discountId).Select(ud => ud.ProductId).ToListAsync();
        }

        public async Task AssignProductDiscountAsync(int productId, int discountId)
        {
            await _context.ProductDiscounts.AddAsync(new ProductDiscount() {
                DiscountId=discountId,
                ProductId= productId
            });
        }
        public async Task AssignUserDiscountAsync(int userId, int discountId)
        {
            await _context.UserDiscounts.AddAsync(new UserDiscount()
            {
                DiscountId = discountId,
                UserId = userId
            });
        }
        public async Task RemoveUserDiscountAsync(int userId,int discountId)
        {
            var user=await _context.UserDiscounts.FirstOrDefaultAsync(_ud => _ud.UserId == userId && _ud.DiscountId == discountId);
            _context.Remove(user);
        }
        public async Task RemoveProductDiscountAsync(int productId, int discountId)
        {
            var product = await _context.ProductDiscounts.FirstOrDefaultAsync(_ud => _ud.ProductId == productId && _ud.DiscountId == discountId);
            _context.Remove(product);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
