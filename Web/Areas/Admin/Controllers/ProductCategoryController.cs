using Application.Services.Interfaces;
using Domain.ViewModel.Product;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class ProductCategoryController : AdminBaseController
    {
        #region Ctor

        private readonly IProductService _productService;

        public ProductCategoryController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region CreateBaseCategory

        [HttpGet]
        public IActionResult CreateBaseCategory()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBaseCategory(BaseCategoryViewModel model)
        {
            #region Validation

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            #endregion

            await _productService.AddBaseCategory(model);
            return RedirectToAction(nameof(BaseCategoryList));
        }

        #endregion

        #region AllBaseCategoryList

        [HttpGet]
        public async Task<IActionResult> BaseCategoryList(int? categoryid=null)
        {
            var model = await _productService.GetAllCategories(categoryid);
            ViewData["parentid"] = categoryid;

            return View(model);
        }

        #endregion

        #region CreateSubCategory

        [HttpGet]
        public IActionResult CreateSubCategory(int parentid)
        {
            var sub = new SubCategoryViewModel()
            {
                ParentId = parentid
            };
            return View(sub);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSubCategory(SubCategoryViewModel model)
        {
            #region Validation

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            #endregion

            await _productService.AddSubCategory(model);

            
            return RedirectToAction(nameof(BaseCategoryList));
        }


        #endregion


   


    }
}
