

using Domain.ViewModel.Product.ProductSpecification;

namespace Application.Services.Interfaces
{
    public interface IProductSpecificationService
    {
        Task AddNewSpecification(AddNewProductSpecification productSpecification , List<string> list);

        Task<List<ShowProductSpecificationViewModel>> GetProductSpecificationList(int productId);
    }
}
