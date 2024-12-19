

using Application.Services.Interfaces;
using Domain.Entities.Product;
using Domain.Interface;
using Domain.ViewModel.Product.ProductSpecification;

namespace Application.Services.Impelementation
{
    public class ProductSpecificationService:IProductSpecificationService
    {
        #region Ctorf

        private readonly IProductSpecificationRepository _productSpecificationRepository;

        public ProductSpecificationService(IProductSpecificationRepository productSpecificationRepository)
        {
            _productSpecificationRepository = productSpecificationRepository;
        }

        #endregion
        public async Task AddNewSpecification(AddNewProductSpecification productSpecification)
        {
            var productSpeci = new ProductSpecification()
            {
                CreateDate = DateTime.Now,
                IsDeleted = false,
                Key = productSpecification.Title,

            };
            await _productSpecificationRepository.AddSpecificationAsync(productSpeci);
            await _productSpecificationRepository.SaveChangeAsync();
            foreach (var item in productSpecification.Values)
            {
                var product = new ProductSpecificationValues()
                {
                    CreateDate = DateTime.Now,
                    ProductSpecificationId = productSpeci.Id,
                    Value = item,
                    IsDeleted = false
                };
                await _productSpecificationRepository.AddSpecificationValues(product);

            }

            await _productSpecificationRepository.SaveChangeAsync();

        }
    }
}
