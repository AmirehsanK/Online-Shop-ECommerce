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
                return await _context.ProductCategories.Where(u => u.ParentId == parentid).ToListAsync();
            }
            return await _context.ProductCategories.Where(u=> u.ParentId==null).ToListAsync();
        }
    }
}
