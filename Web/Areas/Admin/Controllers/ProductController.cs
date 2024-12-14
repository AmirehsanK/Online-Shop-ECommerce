using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class ProductController : AdminBaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            //var model = new ProductViewModel
            //{
            //    Categories = await _productService.GetAllCategories(null)
            //        .Select(c => new SelectListItem
            //        {
            //            Value = c.CategoryId.ToString(),
            //            Text = c.Title
            //        }).ToListAsync()
            //};

            //return View(model);
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _productService.GetAllCategories(null); 
            return Json(categories.Select(c => new { c.CategoryId, c.Title }));
        }

        [HttpGet]
        public async Task<IActionResult> GetSubCategories(int categoryId)
        {
            var subCategories = await _productService.GetAllCategories(categoryId); 
            return Json(subCategories.Select(sc => new { sc.CategoryId, sc.Title }));
        }
    }
}
