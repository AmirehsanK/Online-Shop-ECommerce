using Application.Services.Interfaces;
using Domain.ViewModel.Discount;
using Infra.Data.Statics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Attributes;

namespace Web.Areas.Admin.Controllers;

[InvokePermission(PermissionName.DiscountManagement)]
public class DiscountController(IDiscountService service, IUserService userService, IProductService productService)
    : AdminBaseController
{
    #region Discount List

    [InvokePermission(PermissionName.DiscountList)]
    public async Task<IActionResult> DiscountList()
    {
        var discounts = await service.GetAllAsync();
        return View(discounts);
    }

    #endregion

    #region Crud Discount

    [InvokePermission(PermissionName.CreateDiscount)]
    public IActionResult AddDiscount()
    {
        return View();
    }

    [HttpPost]
    [InvokePermission(PermissionName.CreateDiscount)]
    public async Task<IActionResult> AddDiscount(DiscountViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            TempData[ErrorMessage] = "لطفاً اطلاعات تخفیف را به درستی وارد کنید";
            return View(viewModel);
        }

        await service.AddAsync(viewModel);
        TempData[SuccessMessage] = "تخفیف مورد نظر با موفقیت اضافه شد";
        return RedirectToAction(nameof(DiscountList));
    }

    [InvokePermission(PermissionName.UpdateDiscount)]
    public async Task<IActionResult> Edit(int id)
    {
        var discount = await service.GetByIdAsync(id);
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
    [InvokePermission(PermissionName.UpdateDiscount)]
    public async Task<IActionResult> Edit(int id, DiscountEditViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            TempData[ErrorMessage] = "لطفاً اطلاعات تخفیف را به درستی وارد کنید";
            return View(viewModel);
        }

        await service.UpdateAsync(id, viewModel);
        TempData[SuccessMessage] = "تخفیف مورد نظر با موفقیت ویرایش شد";
        return RedirectToAction(nameof(DiscountList));
    }

    [HttpPost]
    [InvokePermission(PermissionName.DeleteDiscount)]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        TempData[SuccessMessage] = "تخفیف مورد نظر با موفقیت حذف شد";
        return RedirectToAction(nameof(DiscountList));
    }

    #endregion

    #region Assign Discount

    [InvokePermission(PermissionName.AssignToProductDiscount)]
    public async Task<IActionResult> AssignDiscountToProducts(int id)
    {
        var products = await productService.GetAllProductsNoFilter();
        var viewModel = new DiscountProductViewModel
        {
            DiscountId = id,
            SelectedProductIds = await service.GetProductDiscount(id),
            Products = products.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ProductName })
                .ToList()
        };
        TempData["DiscountId"] = id;

        return View(viewModel);
    }

    [HttpPost]
    [InvokePermission(PermissionName.AssignToProductDiscount)]
    public async Task<IActionResult> AssignDiscountToProducts(int discountId, List<int> productIds)
    {
        await service.AssignProductDiscountAsync(productIds, discountId);
        TempData[SuccessMessage] = "تخفیف به محصولات اختصاص یافت";
        return RedirectToAction(nameof(DiscountList));
    }

    [InvokePermission(PermissionName.AssignToUserDiscount)]
    public async Task<IActionResult> AssignDiscountToUsers(int id)
    {
        var users = await userService.GetUserListAsync();
        DiscountUserViewModel viewModel = new()
        {
            DiscountId = id,
            SelectedUserIds = await service.GetUserDiscount(id),
            Users = users.Select(u => new SelectListItem
                { Value = u.Id.ToString(), Text = u.FirstName + " " + u.LastName }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    [InvokePermission(PermissionName.AssignToUserDiscount)]
    public async Task<IActionResult> AssignDiscountToUsers(int discountId, List<int> userIds)
    {
        await service.AssignUserDiscountAsync(userIds, discountId);
        TempData[SuccessMessage] = "تخفیف به کاربران اختصاص یافت";
        return RedirectToAction(nameof(DiscountList));
    }

    #endregion
}