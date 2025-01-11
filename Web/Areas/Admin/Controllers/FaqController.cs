using Application.Services.Interfaces;
using Domain.ViewModel.Faq.Admin;
using Infra.Data.Statics;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;

namespace Web.Areas.Admin.Controllers;

[InvokePermission(PermissionName.FaqManagement)]
public class FaqController(IFaqService faqService) : AdminBaseController
{
    #region FaqCategoryList

    [InvokePermission(PermissionName.FaqList)]
    public async Task<IActionResult> FaqCategoryList()
    {
        var model = await faqService.GetAllFaqCategoriesAsync();
        return View(model);
    }

    #endregion

    #region DeleteFaqQuestionOrCategory

    [HttpGet]
    [InvokePermission(PermissionName.DeleteFaqCategory)]
    public async Task<IActionResult> DeleteFaqCategory(int categoryId)
    {
        await faqService.DeleteFaqCategoryAndChild(categoryId);
        return RedirectToAction(nameof(FaqCategoryList));
    }

    #endregion

    #region FaqQuestionList

    [InvokePermission(PermissionName.FaqList)]
    public async Task<IActionResult> FaqQuestionList(int categoryId)
    {
        var model = await faqService.GetAllFaqQuestionListViewModelsAsync(categoryId);
        return View(model);
    }

    #endregion

    #region DeleteFaqQuestion

    [InvokePermission(PermissionName.DeleteFaq)]
    public async Task<IActionResult> DeleteFaqQuestion(int questionId)
    {
        await faqService.DeleteFaqQuestion(questionId);
        return Redirect(HttpContext.Request.Headers.Referer!);
    }

    #endregion

    #region AddFaqCategory

    [HttpGet]
    [InvokePermission(PermissionName.CreateFaqCategory)]
    public IActionResult AddFaqCategory()
    {
        return View();
    }

    [HttpPost]
    [InvokePermission(PermissionName.CreateFaqCategory)]
    public async Task<IActionResult> AddFaqCategory(AddFaqCategoryViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await faqService.AddFaqCategory(model);
        return RedirectToAction(nameof(FaqCategoryList));
    }

    #endregion

    #region EditFaqCategory

    [HttpGet]
    [InvokePermission(PermissionName.UpdateFaqCategory)]
    public async Task<IActionResult> EditFaqCategory(int categoryId)
    {
        var model = await faqService.GetCategoryForShow(categoryId);
        return View(model);
    }

    [HttpPost]
    [InvokePermission(PermissionName.UpdateFaqCategory)]
    public async Task<IActionResult> EditFaqCategory(EditFaqCategoryViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await faqService.EditFaqCategory(model);
        return RedirectToAction(nameof(FaqCategoryList));
    }

    #endregion

    #region CreateFaqQuestion

    [HttpGet]
    [InvokePermission(PermissionName.CreateFaq)]
    public async Task<IActionResult> CreateFaqQuestion()
    {
        ViewData["Categories"] = await faqService.GetAllCategoryForShow();
        return View();
    }

    [HttpPost]
    [InvokePermission(PermissionName.CreateFaq)]
    public async Task<IActionResult> CreateFaqQuestion(CreateFaqQuestionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Categories"] = await faqService.GetAllCategoryForShow();
            return View(model);
        }

        await faqService.AddFaqQuestion(model);
        ViewData["Categories"] = await faqService.GetAllCategoryForShow();
        return RedirectToAction(nameof(FaqCategoryList));
    }

    #endregion

    #region EditFaqQuestion

    [HttpGet]
    [InvokePermission(PermissionName.UpdateFaq)]
    public async Task<IActionResult> EditFaqQuestion(int questionId)
    {
        var model = await faqService.GetFaqQuestionById(questionId);
        ViewData["Categories"] = await faqService.GetAllCategoryForShow();
        return View(model);
    }

    [HttpPost]
    [InvokePermission(PermissionName.UpdateFaq)]
    public async Task<IActionResult> EditFaqQuestion(EditFaqQuestionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Categories"] = await faqService.GetAllCategoryForShow();
            return View(model);
        }

        await faqService.EditFaqQuestion(model);
        return RedirectToAction(nameof(FaqCategoryList));
    }

    #endregion
}