

using Domain.Entities.Product;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories
{
    public class ProductGalleryRepository:IProductGalleryRepository
    {
        #region Ctor


        private readonly ApplicationDbContext _context;

        public ProductGalleryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task AddGalleryAsync(ProductGallery gallery)
        {
            await _context.ProductGalleries.AddAsync(gallery);
        }

        public async Task<List<ProductGallery>> GetGalleryWithIdAsync(int id)
        {
            return await _context.ProductGalleries.Where(u => u.IsDeleted == false && u.ProductId == id).ToListAsync();
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
