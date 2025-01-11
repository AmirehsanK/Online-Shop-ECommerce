using Domain.Entities.Product;
using Domain.Interface;
using Domain.ViewModel.Product.ProductSpecification;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class ProductSpecificationRepository(ApplicationDbContext context) : IProductSpecificationRepository
{
    #region Save Changes

    public async Task SaveChangeAsync()
    {
        await context.SaveChangesAsync();
    }

    #endregion

    #region Specification Methods

    public async Task AddSpecificationAsync(ProductSpecification productSpecification)
    {
        await context.ProductSpecifications.AddAsync(productSpecification);
    }

    public void UpdateSpecification(ProductSpecification productSpecification)
    {
        context.ProductSpecifications.Update(productSpecification);
    }

    public async Task<List<ProductSpecification>> GetSpecificationAsync(int productId)
    {
        return await context.ProductSpecifications
            .Where(u => u.ProductId == productId)
            .ToListAsync();
    }

    public async Task<FilterProductSpecification> GetProductSpecification(int productid,
        FilterProductSpecification filter)
    {
        var query = context.ProductSpecifications
            .Include(u => u.Product)
            .Where(u => u.ProductId == productid)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter.Title)) query = query.Where(u => u.Key.Contains(filter.Title.Trim()));

        if (!string.IsNullOrEmpty(filter.Value)) query = query.Where(u => u.Value.Contains(filter.Value.Trim()));

        await filter.Paging(query.Select(p => new ShowProductSpecificationViewModel
        {
            ProductId = productid,
            Values = p.Value,
            Id = p.Id,
            Title = p.Key
        }));

        return filter;
    }

    public async Task<ProductSpecification> GetSpecificationById(int SpecificationId)
    {
        return (await context.ProductSpecifications.FindAsync(SpecificationId))!;
    }

    #endregion
}