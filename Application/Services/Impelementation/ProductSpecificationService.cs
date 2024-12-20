

using Application.Services.Interfaces;
using Domain.Entities.Product;
using Domain.Interface;
using Domain.ViewModel.Product.ProductSpecification;
using System.Linq;

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
        public async Task AddNewSpecification(AddNewProductSpecification productSpecification, List<string> list)
        {
            var productSpeci = new ProductSpecification()
            {
                CreateDate = DateTime.Now,
                IsDeleted = false,
                Key = productSpecification.Title,
                ProductId = productSpecification.ProductId

            };
            await _productSpecificationRepository.AddSpecificationAsync(productSpeci);
            await _productSpecificationRepository.SaveChangeAsync();
            foreach (var item in list)
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

        public async Task<List<ShowProductSpecificationViewModel>> GetProductSpecificationList(int productId)
        {
            var spec = await _productSpecificationRepository.GetSpecificationAsync(productId);
            return spec.Select(u => new ShowProductSpecificationViewModel()
            {
                Title = u.ProductSpecification.Key,
                Values = string.Join(",",u.Value)
            }).ToList();
        }
    }
}
