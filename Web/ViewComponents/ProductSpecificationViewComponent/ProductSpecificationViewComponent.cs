using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents.ProductSpecificationViewComponent;

public class ProductSpecificationViewComponent(IProductSpecificationService productSpecificationService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(int ProductId)
    {
        var model = await productSpecificationService.GetProductSpecification(ProductId);
        return View("ProductSpecification", model);
    }
}