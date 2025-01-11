using Application.Services.Interfaces;
using Domain.ViewModel.Product.ProductSpecification;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;
using Infra.Data.Statics;

namespace Web.Areas.Admin.Controllers
{
    [InvokePermission(PermissionName.SpecificationManagement)]
    public class ProductSpecificationController(IProductSpecificationService productSpecificationService)
        : AdminBaseController
    {
        
        #region AddNewSpecificationForProduct

        [HttpGet]
        [InvokePermission(PermissionName.CreateSpecification)]
        public IActionResult AddNewSpecification(int productId)
        {
            return View();
        }

        [HttpPost]
        [InvokePermission(PermissionName.CreateSpecification)]
        public async Task<IActionResult> AddNewSpecification(AddNewProductSpecification productSpecification)
        {
            if (!ModelState.IsValid)
            {
                return View(productSpecification);
            }

            await productSpecificationService.AddNewSpecification(productSpecification);
            ViewBag.Referer = HttpContext.Request.Headers.Referer;
            TempData[SuccessMessage] = $"@{productSpecification.Title} به محصول اضافه شد";
            return Redirect(ViewBag.Referer);
        }

        #endregion

        #region ShowProductSpecification

        [HttpGet]
        [InvokePermission(PermissionName.SpecificationList)]
        public async Task<IActionResult> ShowProductSpecification(int productId, FilterProductSpecification specification)
        {
            specification.TakeEntity = 1;
            var model = await productSpecificationService.GetAllProductSpecificationAsync(specification, productId);
            ViewData["ID"] = productId;
            return View(model);
        }

        #endregion

        #region EditProductSpecification

        [HttpGet]
        [InvokePermission(PermissionName.UpdateSpecification)]
        public async Task<IActionResult> EditProductSpecification(int specificationId)
        {
            var model = await productSpecificationService.GetSpecificationForShow(specificationId);
            model.Value.Split(",");
            return View(model);
        }

        [HttpPost]
        [InvokePermission(PermissionName.UpdateSpecification)]
        [HttpPost]
        public async Task<IActionResult> EditProductSpecification(EditProductSpecificationViewModel model)
        {
            #region Valdaition

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            #endregion

            await productSpecificationService.EditProductSpecification(model);
            return View();
        }

        #endregion
        
    }
}