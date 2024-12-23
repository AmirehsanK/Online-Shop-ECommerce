

using Domain.ViewModel.Product.Product;
using Domain.ViewModel.Product.ProductSpecification;

namespace Application.Services.Interfaces
{
    public interface IProductSpecificationService
    {
        Task AddNewSpecification(AddNewProductSpecification productSpecification);

        Task<FilterProductSpecification> GetAllProductSpecificationAsync(FilterProductSpecification specification, int productid);

        Task EditProductSpecification(EditProductSpecificationViewModel model);

        Task<EditProductSpecificationViewModel> GetSpecificationForShow(int SpecificationId);

        Task<List<ProductSpecificationViewModel>> GetProductSpecification(int productId);
    }
}
