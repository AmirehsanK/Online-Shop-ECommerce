using Application.Services.Interfaces;
using Domain.ViewModel.Product.Product;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ProductController(IProductService productService) : SiteBaseController
    {
        #region ProductList

        [HttpGet("ProductList/")]
        public async Task<IActionResult> ProductList(FilterProductViewModel filter, int SubCategoryId)
        {
            
            var model = await productService.GetAllProductsAsync(filter);
            return View(model);

        }


        #endregion

        #region ProductDetail
        [HttpGet("ProductDetail/{productid}")]
        public async Task<IActionResult> ProductDetail(int productid)
        {
           var model= await productService.GetProductDetailForSite(productid);
            return View(model);
        }

        #endregion
   
    }


}
