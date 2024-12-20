using Application.Services.Interfaces;
using Domain.ViewModel.Product.ProductSpecification;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public IActionResult AddNewSpecification()
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

            return View();
        }
    }
}
