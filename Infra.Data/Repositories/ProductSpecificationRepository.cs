using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Product;
using Domain.Interface;
using Domain.ViewModel.Product.ProductSpecification;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories
{
    public class ProductSpecificationRepository:IProductSpecificationRepository
    {
        #region Ctor

        private readonly ApplicationDbContext _context;

        public ProductSpecificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task AddSpecificationAsync(ProductSpecification productSpecification)
        {
            await _context.ProductSpecifications.AddAsync(productSpecification);
        }

        public void UpdateSpecification(ProductSpecification productSpecification)
        {
            _context.ProductSpecifications.Update(productSpecification);
        }

        public async Task<List<ProductSpecification>> GetSpecificationAsync(int productId)
        {
            return await _context.ProductSpecifications.Where(u => u.ProductId == productId).ToListAsync();

        }

        public async Task<FilterProductSpecification> GetProductSpecification(int productid, FilterProductSpecification filter)
        {
            var query =  _context.ProductSpecifications.Include(u=>u.Product).Where(u => u.ProductId == productid).AsQueryable();
            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(u => u.Key.Contains(filter.Title.Trim()));
            }
            if (!string.IsNullOrEmpty(filter.Value))
            {
                query = query.Where(u => u.Value.Contains(filter.Value.Trim()));
            }
            await filter.Paging(query.Select(p => new ShowProductSpecificationViewModel()
            {
                ProductId = productid,
                Values = p.Value,
                Id  = p.Id , 
                Title = p.Key
                
            }));
            return filter;

        }

        public async Task<ProductSpecification> GetSpecificationById(int SpecificationId)
        {
            return await _context.ProductSpecifications.FindAsync(SpecificationId);
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
