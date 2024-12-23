using Application.Services.Impelementation;
using Application.Services.Interfaces;
using Domain.ViewModel.Discount;
using Domain.ViewModel.Product.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Web.Areas.Admin.Controllers
{
    public class DiscountController(IDiscountService _service,IUserService _userService,IProductService _productService) : AdminBaseController
    {
        [HttpGet]
        public async Task<IActionResult> DiscountList()
        {
            var discounts = await _service.GetAllAsync();
            return View(discounts);
        }
        [HttpGet]
        public IActionResult AddDiscount()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddDiscount(DiscountViewModel viewModel)
        {
            if (!ModelState.IsValid) {
                TempData[ErrorMessage]= "لطفاً اطلاعات تخفیف را به درستی وارد کنید";
                return View(viewModel); }

            await _service.AddAsync(viewModel);
            TempData[SuccessMessage] = "تخفیف مورد نظر با موفقیت اضافه شد";
            return RedirectToAction(nameof(DiscountList));
        }
        [HttpGet]
        public async Task<IActionResult> AssignDiscountToProducts(int id)
        {
            var products = await _productService.GetAllProductsNoFilter();
            //var categories = await _productService.GetAllSubCategories();
            var viewModel = new DiscountProductViewModel
            {
                DiscountId = id,
                SelectedProductIds = await _service.GetProductDiscount(id),
                Products = products.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ProductName }).ToList(),
              //  Categories = categories.Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Title }).ToList()
            };
            TempData["DiscountId"] = id;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AssignDiscountToProducts(int discountId, List<int> productIds)
        {
            await _service.AssignProductDiscountAsync(productIds, discountId);
            TempData[SuccessMessage] = "تخفیف به محصولات اختصاص یافت";
            return RedirectToAction(nameof(DiscountList));
        }
        [HttpGet]
        public async Task<IActionResult> AssignDiscountToUsers(int id)
        {
            var users = await _userService.GetUserListAsync();
            DiscountUserViewModel viewModel = new()
            {
                DiscountId = id,
                SelectedUserIds = await _service.GetUserDiscount(id),
                Users = users.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.FirstName + " " + u.LastName }).ToList(),
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AssignDiscountToUsers(int discountId, List<int> userIds)
        {
            await _service.AssignUserDiscountAsync(userIds, discountId);
            TempData[SuccessMessage] = "تخفیف به کاربران اختصاص یافت";
            return RedirectToAction(nameof(DiscountList));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var discount = await _service.GetByIdAsync(id);
            if (discount == null)
            {
                TempData[ErrorMessage] = "تخفیف مورد نظر یافت نشد";
                return RedirectToAction(nameof(DiscountList));
            }

            var model = new DiscountEditViewModel
            {
                Id = discount.Id,
                Code = discount.Code,
                IsPercentage = discount.IsPercentage,
                Value = discount.Value,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                IsActive = discount.IsActive,
                UsageLimit = discount.UsageLimit
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, DiscountEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "لطفاً اطلاعات تخفیف را به درستی وارد کنید";
                return View(viewModel);
            }
            
            await _service.UpdateAsync(id, viewModel);
            TempData[SuccessMessage] = "تخفیف مورد نظر با موفقیت ویرایش شد";
            return RedirectToAction(nameof(DiscountList));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            TempData[SuccessMessage] = "تخفیف مورد نظر با موفقیت حذف شد";
            return RedirectToAction(nameof(DiscountList));
        }
    }
}
