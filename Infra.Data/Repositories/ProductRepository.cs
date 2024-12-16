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

        #region Category
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

        public async Task<List<ProductCategory>> GetAllSubCategory()
        { 
            return await _context.ProductCategories.Where(u => u.ParentId != null && u.IsDeleted == false).ToListAsync();
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


        #endregion

        #region Product

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Product.ToListAsync();
        }

        public async Task<Product> GetProductById(int ProductId)
        {
            return await _context.Product.FirstOrDefaultAsync(u => u.Id == ProductId);
        }

        public async Task AddProductAsync(Product product)
        {
           await _context.AddAsync(product);
        }

        public async Task UpdateProduct(Product product)
        {
            _context.Update(product);
        }
        //TODO
        public async Task DeleteProduct(int ProductId)
        {
            
        }

        #endregion
    }
}
