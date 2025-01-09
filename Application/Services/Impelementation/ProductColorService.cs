using Application.Services.Interfaces;
using Domain.Entities.Product;
using Domain.Enums;
using Domain.Interface;
using Domain.ViewModel.Product.ProductColor;

namespace Application.Services.Impelementation;

public class ProductColorService(IProductColorRepository productColorRepository) : IProductColorService
{

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
        await productColorRepository.AddColorAsync(newColor);
        await productColorRepository.SaveChangeAsync();
    }

    #endregion

    #region ColorList

    public async Task<List<ColorListViewModel>> GetColorList()
    {
        var colors = await productColorRepository.GetAllColorAsync();

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
        var color = await productColorRepository.GetColorById(colorId);
        color.IsDeleted = true;
        productColorRepository.UpdateColor(color);
        await productColorRepository.SaveChangeAsync();
    }

    #endregion

    #region AddColorToProduct

    public async Task<ProductExistColor> AddColorToProduct(AddProductColorViewModel productColor,int productid)
    {
        var color = await productColorRepository.GetColorById(productColor.Colorid);
        if (await productColorRepository.CheckIsColorExistForProduct(productid, color.ColorCode))
        {
            return ProductExistColor.Exist;
        }
        var newProductColor = new ProductColor()
        {
            CreateDate = DateTime.Now,
            ColorId = productColor.Colorid,
            IsDeleted = false,
            Count = productColor.Count,
            Price = productColor.Price,
            ProductId = productid
        };
        await productColorRepository.AddProductColorAsync(newProductColor);
        await productColorRepository.SaveChangeAsync();
        return ProductExistColor.NotFound;
    }

    #endregion
  
}