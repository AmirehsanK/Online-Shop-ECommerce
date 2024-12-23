
using Domain.Entities.Product;
using Domain.ViewModel.Product.ProductSpecification;

namespace Domain.Interface
{
    public interface IProductSpecificationRepository
    {
        Task AddSpecificationAsync(ProductSpecification productSpecification);


        void UpdateSpecification(ProductSpecification productSpecification);

        Task<List<ProductSpecification>> GetSpecificationAsync(int productId);

        Task<FilterProductSpecification> GetProductSpecification(int productid,FilterProductSpecification filter);

        Task<ProductSpecification> GetSpecificationById(int SpecificationId);


        Task SaveChangeAsync();
    }
}
