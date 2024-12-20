using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Product;
using Domain.Interface;
using Infra.Data.Context;

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

        public async Task AddSpecificationValues(ProductSpecificationValues productSpecificationValues)
        {
            await _context.ProductSpecificationValuesEnumerable.AddAsync(productSpecificationValues);
        }

        public void UpdateSpecification(ProductSpecification productSpecification)
        {
            _context.ProductSpecifications.Update(productSpecification);
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
