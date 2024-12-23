using Application.Services.Interfaces;
using Domain.ViewModel.Discount;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers;

namespace Web.Areas.Admin.Controllers
{
    public class DiscountController(IDiscountService _service) : AdminBaseController
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

        public async Task<IActionResult> Edit(int id)
        {
            var discount = await _service.GetByIdAsync(id);
            if (discount == null) {
                TempData[ErrorMessage] = "تخفیف مورد نظر یافت نشد";
                return RedirectToAction(nameof(DiscountList)); }

            return View(discount);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, DiscountViewModel viewModel)
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
            TempData[SuccessMessage]= "تخفیف مورد نظر با موفقیت حذف شد";
            return RedirectToAction(nameof(DiscountList));
        }
    }
}
