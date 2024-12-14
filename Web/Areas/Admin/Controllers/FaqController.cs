using Application.Services.Interfaces;
using Domain.ViewModel.Faq.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
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

        public async Task<IActionResult> FaqCategoryList()
        {
            var model = await _faqService.GetAllFaqCategoriesAsync();

            return View(model);
        }

        #endregion

        #region AddFaqCategory

        [HttpGet]
        public async Task<IActionResult> AddFaqCategory()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddFaqCategory(AddFaqCategoryViewModel model)
        {
            #region Validation

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            #endregion

            await _faqService.AddFaqCategory(model);
            return RedirectToAction(nameof(FaqCategoryList));
        }

        #endregion

        #region EditFaqCategory
        [HttpGet]
        public async Task<IActionResult> EditFaqCategory(int categoryid)
        {
            var model = await _faqService.GetCategoryForShow(categoryid);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditFaqCategory(EditFaqCategoryViewModel model)
        {
            #region Validation

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            #endregion

            await _faqService.EditFaqCategory(model);
            return View();
        }

        #endregion

        #region DeleteFaqQuestionOrCategory

        [HttpGet]
        public async Task<IActionResult> DeleteFaqCategory(int categoryid)
        {
            await _faqService.DeleteFaqCategoryAndChild(categoryid);
            return RedirectToAction(nameof(FaqCategoryList));
        }

        #endregion

        #region CreateFaqQuestion

        [HttpGet]
        public async Task<IActionResult> CreateFaqQuestion()
        {
            ViewData["Categories"] = await _faqService.GetAllCategoryForShow();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFaqQuestion(CreateFaqQuestionViewModel model)
        {
            #region Validation

            if (!ModelState.IsValid)
            {
                ViewData["Categories"] = await _faqService.GetAllCategoryForShow();
                return View(model);
            }

            #endregion

            await _faqService.AddFaqQuestion(model);
            ViewData["Categories"] = await _faqService.GetAllCategoryForShow();
            return View();
        }

        #endregion

        #region FAqQuestionList

        public async Task<IActionResult> FaqQuestionList(int categoryid)
        {
            var model = await _faqService.GetAllFaqQuestionListViewModelsAsync(categoryid);
            return View(model);
        }

        #endregion

        #region EditFaqQuestion

        [HttpGet]
        public async Task<IActionResult> EditFaqQuestion(int questionId)
        {
            var model = await _faqService.GetFaqQuestionById(questionId);
            ViewData["Categories"] = await _faqService.GetAllCategoryForShow();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditFaqQuestion(EditFaqQuestionViewModel model)
        {
            #region Validation

            if (!ModelState.IsValid)
            {
                ViewData["Categories"] = await _faqService.GetAllCategoryForShow();
                return View(model);
            }


            #endregion

            await _faqService.EditFaqQuestion(model);

            return RedirectToAction(nameof(FaqCategoryList));
        }

        #endregion

        public async Task<IActionResult> DeleteFaqQuestion(int queestionid)
        {
            await _faqService.DeleteFaqQuestion(queestionid);
             
            return Redirect(HttpContext.Request.Headers.Referer);
        }



    }
}
