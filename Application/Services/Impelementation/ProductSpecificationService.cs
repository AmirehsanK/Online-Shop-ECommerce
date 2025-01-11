using Application.Services.Interfaces;
using Domain.Entities.Product;
using Domain.Interface;
using Domain.ViewModel.Product.ProductSpecification;

namespace Application.Services.Impelementation;

public class ProductSpecificationService(IProductSpecificationRepository productSpecificationRepository)
    : IProductSpecificationService
{
    #region Add Specification

    public async Task AddNewSpecification(AddNewProductSpecification productSpecification)
    {
        var productSpec = new ProductSpecification
        {
            CreateDate = DateTime.Now,
            IsDeleted = false,
            Key = productSpecification.Title,
            ProductId = productSpecification.ProductId,
            Value = productSpecification.Values
        };
        await productSpecificationRepository.AddSpecificationAsync(productSpec);
        await productSpecificationRepository.SaveChangeAsync();
    }

    #endregion

    #region Get Specifications

    public async Task<FilterProductSpecification> GetAllProductSpecificationAsync(
        FilterProductSpecification specification, int productid)
    {
        return await productSpecificationRepository.GetProductSpecification(productid, specification);
    }

    public async Task<List<ProductSpecificationViewModel>> GetProductSpecification(int productId)
    {
        var specification = await productSpecificationRepository.GetSpecificationAsync(productId);
        return specification.Select(u => new ProductSpecificationViewModel
        {
            Value = u.Value,
            key = u.Key
        }).ToList();
    }

    #endregion

    #region Edit Specification

    public async Task EditProductSpecification(EditProductSpecificationViewModel model)
    {
        var specification = await productSpecificationRepository.GetSpecificationById(model.Id);
        specification.Key = model.Key;
        specification.Value = model.Value;
        productSpecificationRepository.UpdateSpecification(specification);
        await productSpecificationRepository.SaveChangeAsync();
    }

    public async Task<EditProductSpecificationViewModel> GetSpecificationForShow(int specificationId)
    {
        var spec = await productSpecificationRepository.GetSpecificationById(specificationId);
        var viewmodel = new EditProductSpecificationViewModel
        {
            Key = spec.Key,
            Value = spec.Value,
            Id = spec.Id
        };
        return viewmodel;
    }

    #endregion
}