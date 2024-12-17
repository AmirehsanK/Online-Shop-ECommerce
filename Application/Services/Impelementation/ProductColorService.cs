using Application.Services.Interfaces;
using Domain.Entities.Product;
using Domain.Interface;
using Domain.ViewModel.Product.ProductColor;

namespace Application.Services.Impelementation
{
    public class ProductColorService : IProductColorService
    {
        #region Ctor

        private readonly IProductColorRepository _productColorRepository;

        public ProductColorService(IProductColorRepository productColorRepository)
        {
            _productColorRepository = productColorRepository;
        }



        #endregion

        #region AddNewColor

        public async Task AddNewColor(CreateProductColorViewModel color)
        {
            var newColor = new Color()
            {
                CreateDate = DateTime.Now,
                Title = color.Title,
                IsDeleted = false,
                ColorCode = color.ColorCode,

            };
            await _productColorRepository.AddColorAsync(newColor);
            await _productColorRepository.SaveChangeAsync();
        }

        #endregion

        #region ColorList

        public async Task<List<ColorListViewModel>> GetColorList()
        {
            var colors = await _productColorRepository.GetAllColorAsync();

            return colors.Select(u => new ColorListViewModel()
            {

                CreateDate = u.CreateDate,
                Title = u.Title,
                ColorCode = u.ColorCode,
                Id = u.Id
            }).ToList();
        }

        #endregion

        #region DeleteColor

        public async Task DeleteColorAsync(int colorId)
        {
            var color = await _productColorRepository.GetColorById(colorId);
            color.IsDeleted = true;
            _productColorRepository.UpdateColor(color);
            await _productColorRepository.SaveChangeAsync();
        }

        #endregion

        #region AddColorToProduct

        public async Task AddProductToGallery(AddProductColorViewModel productColor,int productid)
        {
            var newproductColor = new ProductColor()
            {
                CreateDate = DateTime.Now,
                ColorId = productColor.Colorid,
                IsDeleted = false,
                Count = productColor.Count,
                Price = productColor.Price,
                ProductId = productid
            };
            await _productColorRepository.AddProductColorAsync(newproductColor);
            await _productColorRepository.SaveChangeAsync();
        }

        #endregion
 
  
    }
}
