using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Product;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories
{
    public class ProductRepository:IProductRepository
    {
        #region Ctor

        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

      
        public async Task AddCategoryAsync(ProductCategory product)
        {
            await _context.ProductCategories.AddAsync(product);
        }

        public void UpdateCategoryAsync(ProductCategory product)
        {
            _context.ProductCategories.Update(product);
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductCategory>> GetAllCategory(int? parentid = null)
        {
            if (parentid.HasValue)
            {
                return await _context.ProductCategories.Where(u => u.ParentId == parentid&&u.IsDeleted==false).ToListAsync();
            }
            return await _context.ProductCategories.Where(u=> u.ParentId==null&&u.IsDeleted==false).ToListAsync();
        }

        public async Task<ProductCategory> GetBaseCategory(int CategoryId)
        {
            return await _context.ProductCategories.Where(u=>u.IsDeleted==false).FirstOrDefaultAsync(u=> u.Id==CategoryId);
        }

        public async Task<List<ProductCategory>> GetSubCategory(int CategoryId)
        {
            return await _context.ProductCategories.Where(u => u.ParentId == CategoryId).ToListAsync();
        }

        public void UpdateCategoryList(List<ProductCategory> model)
        {
            _context.ProductCategories.UpdateRange(model);
        }
    }
}
