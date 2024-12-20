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

            var list = new List<string>(productSpecification.Values.Split(","));
           

            await _productSpecificationService.AddNewSpecification(productSpecification,list);
            ViewBag.Referer = HttpContext.Request.Headers.Referer;
            TempData[SuccessMessage] = $"@{productSpecification.Title}به محصول اضافه شد";
            return Redirect(ViewBag.Referer);
        }

        #endregion

        #region ShowProductSpecification


        [HttpGet]
        public async Task<IActionResult> ShowProductSpecification(int productid)
        {
            var model = await _productSpecificationService.GetProductSpecificationList(productid);
            return View(model);
        }


        #endregion

    }
}
