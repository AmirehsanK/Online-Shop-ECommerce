

using Domain.ViewModel.Product.ProductSpecification;

namespace Application.Services.Interfaces
{
    public interface IProductSpecificationService
    {
        Task AddNewSpecification(AddNewProductSpecification productSpecification);
    }
}
