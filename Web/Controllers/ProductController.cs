using Application.Services.Interfaces;
using Domain.ViewModel.Product.Product;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ProductController : SiteBaseController
    {
        #region Ctor

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion
    
        [HttpGet()]
        public async Task<IActionResult> ProductList(FilterProductViewModel filter,string SubCategoryTitle)
        {
            var model = await _productService.GetAllProductsAsync(filter);
            return View(model);
        }
    }
}
