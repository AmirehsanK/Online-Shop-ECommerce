using Application.Extention;
using Application.Services.Interfaces;
using Application.Tools;
using Domain.Entities.Product;
using Domain.Interface;
using Domain.ViewModel.Product.ProductGallery;

namespace Application.Services.Impelementation;

public class ProductGalleryService(IProductGalleryRepository productGalleryRepository) : IProductGalleryService
{
        
    #region Get Gallery
    public async Task<List<ProductGallery>> GetGalleryListAsync(int productid)
    {
        return await productGalleryRepository.GetGalleryWithIdAsync(productid);
    }
    #endregion

    #region Add Gallery
    public async Task AddProductGalleries(ShowProductGalleryViewModel model)
    {
        if (model.Gallery != null && model.Gallery.Count != 0)
        {
            foreach (var item in model.Gallery)
            {
                var galleryImageName = Guid.NewGuid().ToString("N") + Path.GetExtension(item.FileName);
                item.AddImageToServer(galleryImageName, PathTools.ProductGalleryImageServerPath, 100, 70, PathTools.ProductGalleryThumbImageServerPath);
                var newGallery = new ProductGallery()
                {
                    CreateDate = DateTime.Now,
                    ProductId = model.ProductId,
                    IsDeleted = false,
                    Image = galleryImageName,
                };
                await productGalleryRepository.AddGalleryAsync(newGallery);
            }

            await productGalleryRepository.SaveChangeAsync();
        }
    }
    #endregion

    #region Remove Gallery
    public async Task RemoveProductGallery(int galleryid)
    {
        var photo = await productGalleryRepository.GetOneGalleryWithIdAsync(galleryid);
        photo.Image.DeleteImage(PathTools.ProductGalleryImageServerPath, PathTools.ProductGalleryThumbImageServerPath);
        productGalleryRepository.RemoveProductGallery(photo);
        await productGalleryRepository.SaveChangeAsync();
    }
    #endregion
        
}