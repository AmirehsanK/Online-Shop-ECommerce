using Application.Services.Interfaces;
using Domain.ViewModel.Product.CategoryAdmin;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;
using Infra.Data.Statics;

namespace Web.Areas.Admin.Controllers
{
    [InvokePermission(PermissionName.CategoryManagement)]
    public class ProductCategoryController(IProductService productService) : AdminBaseController
    {

        #region CreateBaseCategory

        [HttpGet]
        [InvokePermission(PermissionName.CreateBaseCategory)]
        public IActionResult CreateBaseCategory()
        {
            return View();
        }

        [HttpPost]
        [InvokePermission(PermissionName.CreateBaseCategory)]
        public async Task<IActionResult> CreateBaseCategory(BaseCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await productService.AddBaseCategory(model);
            TempData[SuccessMessage] = "دسته‌بندی پایه با موفقیت ایجاد شد.";
            return RedirectToAction(nameof(BaseCategoryList));
        }

        #endregion

        #region EditBaseCategory

        [HttpGet]
        [InvokePermission(PermissionName.UpdateCategory)]
        public async Task<IActionResult> EditBaseCategory(int categoryId)
        {
            var model = await productService.GetBaseCategoryForEdit(categoryId);
            return View(model);
        }

        [HttpPost]
        [InvokePermission(PermissionName.UpdateCategory)]
        public async Task<IActionResult> EditBaseCategory(EditBaseCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await productService.EditBaseCategory(model);
            TempData[SuccessMessage] = "دسته‌بندی پایه با موفقیت ویرایش شد.";
            return RedirectToAction(nameof(BaseCategoryList));
        }

        #endregion

        #region AllBaseCategoryList

        [HttpGet]
        [InvokePermission(PermissionName.CategoryList)]
        public async Task<IActionResult> BaseCategoryList(int? categoryId = null)
        {
            var model = await productService.GetAllCategories(categoryId);
            ViewData["parentid"] = categoryId;

            return View(model);
        }

        #endregion

        #region CreateSubCategory

        [HttpGet]
        [InvokePermission(PermissionName.CreateSubCategory)]
        public IActionResult CreateSubCategory(int parentId)
        {
            var sub = new SubCategoryViewModel()
            {
                ParentId = parentId
            };
            return View(sub);
        }

        [HttpPost]
        [InvokePermission(PermissionName.CreateSubCategory)]
        public async Task<IActionResult> CreateSubCategory(SubCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await productService.AddSubCategory(model);
            TempData[SuccessMessage] = "زیردسته‌بندی با موفقیت ایجاد شد.";
            return RedirectToAction(nameof(BaseCategoryList));
        }

        #endregion

        #region DeleteBaseCategory

        [HttpGet]
        [InvokePermission(PermissionName.DeleteCategory)]
        public async Task<IActionResult> DeleteBaseCategory(int categoryId)
        {
            await productService.DeleteBaseCategory(categoryId);
            TempData[SuccessMessage] = "دسته‌بندی پایه با موفقیت حذف شد.";
            return RedirectToAction(nameof(BaseCategoryList));
        }

        #endregion
    }
}