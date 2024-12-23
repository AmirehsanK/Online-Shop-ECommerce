
using Domain.Enums;
using Domain.ViewModel.Product.ProductColor;

namespace Application.Services.Interfaces
{
    public interface IProductColorService
    {
        Task AddNewColor(CreateProductColorViewModel  color);

        Task<List<ColorListViewModel>> GetColorList();

        Task DeleteColorAsync(int  colorId);
        Task<ProductExistColor> AddColorToProduct(AddProductColorViewModel productColor,int productid);
    }
}
