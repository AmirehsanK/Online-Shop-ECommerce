
using Domain.Entities.Product;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories
{
    public class ProductColorRepository:IProductColorRepository
    {
        #region Ctor

        private readonly ApplicationDbContext _context;

        public ProductColorRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        #endregion

        public async Task AddColorAsync(Color color)
        {
            await _context.Colors.AddAsync(color);
        }

        public void UpdateColor(Color color)
        {
            _context.Colors.Update(color);
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddProductColorAsync(ProductColor color)
        {
            await _context.ProductColors.AddAsync(color);
        }

        public void UpdateProductColor(ProductColor color)
        {
            _context.Update(color);
        }

        public async Task<List<Color>> GetAllColorAsync()
        {
           return await _context.Colors.Where(u=> u.IsDeleted==false).ToListAsync();
        }

        public async Task<bool> CheckIsColorExistForProduct(int productId, string colorCode)
        {
            return await _context.ProductColors.Where(u => u.ProductId == productId)
                .Include(u => u.Color)
                .Where(u => u.Color.ColorCode == colorCode).AnyAsync();
        }

        public async Task<ProductColor> GetProductColorWithid(int productColorid)
        {
            return await _context.ProductColors.Include(u=> u.Color).FirstOrDefaultAsync(u=> u.Id==productColorid);
        }

        public async Task<List<ProductColor>> GetProductColorAsync(int productId)
        {
            return await _context.ProductColors.Where(u => u.ProductId == productId).Include(u => u.Color)
                .ToListAsync();
        }

        

        public async Task<Color> GetColorById(int id)
        {
            return await _context.Colors.FindAsync(id);
        }
    }
}
