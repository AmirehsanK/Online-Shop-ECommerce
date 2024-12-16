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

        public async Task DeleteColorAsync(int colorId)
        {
            var color = await _productColorRepository.GetColorById(colorId);
            color.IsDeleted = true;
            _productColorRepository.UpdateColor(color);
            await _productColorRepository.SaveChangeAsync();
        }
    }
}
