using Domain.Entities.Product;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class ProductGalleryRepository(ApplicationDbContext context) : IProductGalleryRepository
{
    #region Save Changes

    public async Task SaveChangeAsync()
    {
        await context.SaveChangesAsync();
    }

    #endregion

    #region Gallery Methods

    public async Task AddGalleryAsync(ProductGallery gallery)
    {
        await context.ProductGalleries.AddAsync(gallery);
    }

    public async Task<List<ProductGallery>> GetGalleryWithIdAsync(int id)
    {
        return await context.ProductGalleries
            .Where(u => u.IsDeleted == false && u.ProductId == id)
            .ToListAsync();
    }

    public async Task<ProductGallery> GetOneGalleryWithIdAsync(int id)
    {
        return (await context.ProductGalleries.FindAsync(id))!;
    }

    public void RemoveProductGallery(ProductGallery gallery)
    {
        context.ProductGalleries.Remove(gallery);
    }

    #endregion
}