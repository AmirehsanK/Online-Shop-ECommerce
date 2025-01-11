using Domain.Enums;
using Domain.ViewModel.Product.ProductColor;

namespace Application.Services.Interfaces;

public interface IProductColorService
{
    #region Product Color Management

    Task<ProductExistColor> AddColorToProduct(AddProductColorViewModel productColor, int productid);

    #endregion

    #region Color Management

    Task AddNewColor(CreateProductColorViewModel color);
    Task<List<ColorListViewModel>> GetColorList();
    Task DeleteColorAsync(int colorId);

    #endregion
}