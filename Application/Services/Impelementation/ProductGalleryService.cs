using Application.Extention;
using Application.Services.Interfaces;
using Application.Tools;
using Domain.Entities.Product;
using Domain.Interface;
using Domain.ViewModel.Product.ProductColor;
using Domain.ViewModel.Product.ProductGallery;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Impelementation
{
    public class ProductGalleryService:IProductGalleryService
    {
        #region Ctor

        private readonly IProductGalleryRepository _productGalleryRepository;

        public ProductGalleryService(IProductGalleryRepository productGalleryrepository)
        {
            _productGalleryRepository = productGalleryrepository;
        }

        #endregion
        public async Task<List<ProductGallery>> GetGalleryListAsync(int productid)
        {
            return  await _productGalleryRepository.GetGalleryWithIdAsync(productid);
     
        }

      

        public async Task AddProductGalleries(ShowProductGalleryViewModel model)
        {
            if (model.Gallery!=null&&model.Gallery.Any())
            {


                foreach (var item in model.Gallery)
                {
                    var galleryimagename=Guid.NewGuid().ToString("N")+Path.GetExtension(item.FileName);
                    item.AddImageToServer(galleryimagename,PathTools.ProductGalleryImageServerPath,100,70,PathTools.ProductGalleryThumbImageServerPath);
                    var newgallery = new ProductGallery()
                    {
                        CreateDate = DateTime.Now,
                        ProductId = model.ProductId,
                        IsDeleted = false,
                        Image = galleryimagename,

                    };
                    await _productGalleryRepository.AddGalleryAsync(newgallery);

                }

                await _productGalleryRepository.SaveChangeAsync();


            }
        }

        public async Task RemoveProductGallery(int galleryid)
        {
            var photo = await _productGalleryRepository.GetOneGalleryWithIdAsync(galleryid);
            photo.Image.DeleteImage(PathTools.ProductGalleryImageServerPath, PathTools.ProductGalleryThumbImageServerPath);
             _productGalleryRepository.RemoveProductGallery(photo);
             await _productGalleryRepository.SaveChangeAsync();
        }

     
    }
}
