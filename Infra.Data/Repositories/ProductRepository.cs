using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Product;
using Domain.Interface;
using Domain.ViewModel.Product.Product;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;


namespace Infra.Data.Repositories
{
    public class ProductRepository : IProductRepository
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

        public async Task<List<ProductCategory>> GetAllCategoriesAsync()
        {
            return await _context.ProductCategories.Where(u => u.IsDeleted == false).ToListAsync();
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

        public async Task<FilterProductViewModel> GetProductsAsync(FilterProductViewModel filter)
        {
            var query = _context.Product.Include(p => p.Category).Where(p => !p.IsDeleted).AsQueryable();
            if (filter.Inventory.HasValue)
            {
                query = query.Where(_ => _.Inventory == filter.Inventory);
            }
            if (!string.IsNullOrEmpty(filter.ProductName))
            {
                query = query.Where(_ => _.ProductName.Contains(filter.ProductName.Trim()));
            }
            if (filter.Price.HasValue) {
                query = query.Where(_ => _.ProductName.Contains(filter.ProductName.Trim()));
            }
            if (filter.StartPrice != null)
                query = query.Where(p => p.Price >= filter.StartPrice);

            if (filter.EndPrice != null)
                query = query.Where(p => p.Price <= filter.EndPrice);
            if (filter.SubCategoryId.HasValue)
                query = query.Where(u => u.Category.Id== filter.SubCategoryId);
            await filter.Paging(query.Select(p => new ProductViewModel()
            {
                ImageName = p.ImageName,
                Id  = p.Id , 
                Inventory = p.Inventory,
                ProductName = p.ProductName,
                SubCategoryId = p.CategoryId,
                Price = p.Price,
            }));
            return filter;
        }

        public async Task<Product?> GetProductById(int ProductId)
        {
            return await _context.Product.Where(u=>u.Id==ProductId)
                .Include(u => u.ProductColors)
                .ThenInclude(u => u.Color)
                .Include(u=> u.ProductGalleries).FirstOrDefaultAsync();
        }

        public async Task AddProductAsync(Product product)
        {
           await _context.AddAsync(product);
        }

        public void UpdateProduct(Product product)
        {
            _context.Update(product);
        }

        public void DeleteProduct(Product product)
        {
            _context.Update(product);
        }

        #endregion
    }
}
