using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Product;

namespace Domain.Interface
{
    public interface IProductSpecificationRepository
    {
        Task AddSpecificationAsync(ProductSpecification productSpecification);

        Task AddSpecificationValues(ProductSpecificationValues productSpecificationValues);

        void UpdateSpecification(ProductSpecification productSpecification);

        Task<List<ProductSpecificationValues>> GetSpecificationAsync(int productId);


        Task SaveChangeAsync();
    }
}
