using Application.Services.Interfaces;
using Domain.ViewModel.Product.ProductSpecification;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Web.Areas.Admin.Controllers
{
    public class ProductSpecificationController : AdminBaseController
    {
        #region Ctor

        private readonly IProductSpecificationService _productSpecificationService;

        public ProductSpecificationController(IProductSpecificationService productSpecificationService)
        {
            _productSpecificationService = productSpecificationService;
        }

        #endregion

        #region AddNewSpecificationForProduct

        [HttpGet]
        public IActionResult AddNewSpecification(int productId)
        {
            return View();
        }
        
        [HttpPost]
        public async  Task<IActionResult> AddNewSpecification(AddNewProductSpecification productSpecification)
        {
            #region Validation

            if (!ModelState.IsValid)
            {
                return View(productSpecification);
            }

            #endregion

           

            await _productSpecificationService.AddNewSpecification(productSpecification);
            ViewBag.Referer = HttpContext.Request.Headers.Referer;
            TempData[SuccessMessage] = $"@{productSpecification.Title}به محصول اضافه شد";
            return Redirect(ViewBag.Referer);
        }

        #endregion

        #region ShowProductSpecification


        [HttpGet]
        public async Task<IActionResult> ShowProductSpecification(int productid,FilterProductSpecification specification)
        {
            specification.TakeEntity = 1;
            var model = await _productSpecificationService.GetAllProductSpecificationAsync(specification, productid);
            ViewData["ID"] = productid;
            return View(model);
        }


        #endregion

        #region EditProductSpecification

        [HttpGet]
        public async Task<IActionResult> EditProductSpecification(int SpecificationId)
        {
          var model=  await _productSpecificationService.GetSpecificationForShow(SpecificationId);
              model.Value.Split(",");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditProductSpecification(EditProductSpecificationViewModel model)
        {
            #region Valdaition

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            #endregion

            await _productSpecificationService.EditProductSpecification(model);
            return View();
        }
        #endregion

    }
}
