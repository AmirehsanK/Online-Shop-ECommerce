using Application.Services.Interfaces;
using Domain.ViewModel.Faq.Admin;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;
using Infra.Data.Statics;

namespace Web.Areas.Admin.Controllers
{
    [InvokePermission(PermissionName.FaqManagement)]
    public class FaqController : AdminBaseController
    {
        #region Ctor

        private readonly IFaqService _faqService;

        public FaqController(IFaqService faqService)
        {
            _faqService = faqService;
        }

        #endregion

        #region FaqCategoryList

        [InvokePermission(PermissionName.FaqList)]
        public async Task<IActionResult> FaqCategoryList()
        {
            var model = await _faqService.GetAllFaqCategoriesAsync();
            return View(model);
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _faqService.AddFaqCategory(model);
            return RedirectToAction(nameof(FaqCategoryList));
        }

        #endregion

        #region EditFaqCategory

        [HttpGet]
        [InvokePermission(PermissionName.UpdateFaqCategory)]
        public async Task<IActionResult> EditFaqCategory(int categoryid)
        {
            var model = await _faqService.GetCategoryForShow(categoryid);
            return View(model);
        }

        [HttpPost]
        [InvokePermission(PermissionName.UpdateFaqCategory)]
        public async Task<IActionResult> EditFaqCategory(EditFaqCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _faqService.EditFaqCategory(model);
            return RedirectToAction(nameof(FaqCategoryList));
        }

        #endregion

        #region DeleteFaqQuestionOrCategory

        [HttpGet]
        [InvokePermission(PermissionName.DeleteFaqCategory)]
        public async Task<IActionResult> DeleteFaqCategory(int categoryid)
        {
            await _faqService.DeleteFaqCategoryAndChild(categoryid);
            return RedirectToAction(nameof(FaqCategoryList));
        }

        #endregion

        #region CreateFaqQuestion

        [HttpGet]
        [InvokePermission(PermissionName.CreateFaq)]
        public async Task<IActionResult> CreateFaqQuestion()
        {
            ViewData["Categories"] = await _faqService.GetAllCategoryForShow();
            return View();
        }

        [HttpPost]
        [InvokePermission(PermissionName.CreateFaq)]
        public async Task<IActionResult> CreateFaqQuestion(CreateFaqQuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Categories"] = await _faqService.GetAllCategoryForShow();
                return View(model);
            }

            await _faqService.AddFaqQuestion(model);
            ViewData["Categories"] = await _faqService.GetAllCategoryForShow();
            return RedirectToAction(nameof(FaqCategoryList));
        }

        #endregion

        #region FaqQuestionList

        [InvokePermission(PermissionName.FaqList)]
        public async Task<IActionResult> FaqQuestionList(int categoryid)
        {
            var model = await _faqService.GetAllFaqQuestionListViewModelsAsync(categoryid);
            return View(model);
        }

        #endregion

        #region EditFaqQuestion

        [HttpGet]
        [InvokePermission(PermissionName.UpdateFaq)]
        public async Task<IActionResult> EditFaqQuestion(int questionId)
        {
            var model = await _faqService.GetFaqQuestionById(questionId);
            ViewData["Categories"] = await _faqService.GetAllCategoryForShow();
            return View(model);
        }

        [HttpPost]
        [InvokePermission(PermissionName.UpdateFaq)]
        public async Task<IActionResult> EditFaqQuestion(EditFaqQuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Categories"] = await _faqService.GetAllCategoryForShow();
                return View(model);
            }

            await _faqService.EditFaqQuestion(model);
            return RedirectToAction(nameof(FaqCategoryList));
        }

        #endregion

        [InvokePermission(PermissionName.DeleteFaq)]
        public async Task<IActionResult> DeleteFaqQuestion(int queestionid)
        {
            await _faqService.DeleteFaqQuestion(queestionid);
            return Redirect(HttpContext.Request.Headers.Referer);
        }
    }
}