

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
        public async Task AddNewSpecification(AddNewProductSpecification productSpecification)
        {
            var productSpeci = new ProductSpecification()
            {
                CreateDate = DateTime.Now,
                IsDeleted = false,
                Key = productSpecification.Title,
                ProductId = productSpecification.ProductId,
                Value = productSpecification.Values
            };
            await _productSpecificationRepository.AddSpecificationAsync(productSpeci);
            await _productSpecificationRepository.SaveChangeAsync();

        }

        public async Task<FilterProductSpecification> GetAllProductSpecificationAsync(FilterProductSpecification specification, int productid)
        {
            return await _productSpecificationRepository.GetProductSpecification(productid, specification);
        }

        public async Task EditProductSpecification(EditProductSpecificationViewModel model)
        {
            var specification = await _productSpecificationRepository.GetSpecificationById(model.Id);
            specification.Key = model.Key;
            specification.Value=model.Value;
            _productSpecificationRepository.UpdateSpecification(specification);
            await _productSpecificationRepository.SaveChangeAsync();

        }

        public async Task<EditProductSpecificationViewModel> GetSpecificationForShow(int SpecificationId)
        {
            var Spec = await _productSpecificationRepository.GetSpecificationById(SpecificationId);
            var viewmodel = new EditProductSpecificationViewModel()
            {
                Key = Spec.Key,
                Value = Spec.Value,
                Id = Spec.Id
            };
            return viewmodel;
        }

        public async Task<List<ProductSpecificationViewModel>> GetProductSpecification(int productId)
        {
            var Specification = await _productSpecificationRepository.GetSpecificationAsync(productId);
            return Specification.Select(u => new ProductSpecificationViewModel()
            {
                Value = u.Value,
                key = u.Key
            }).ToList();
        }
    }
}
