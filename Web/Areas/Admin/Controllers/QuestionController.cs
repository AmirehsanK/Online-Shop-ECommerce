using Application.Services.Interfaces;
using Domain.ViewModel.Question;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class QuestionController(IQuestionService questionService) : AdminBaseController
    {
        #region QuestionList

        public async Task<IActionResult> QuestionList(FilterQuestionListViewModel filter)
        {
            var model = await questionService.GetQuestion(filter);
            return View(model);
        }

        #endregion


        #region QuestionDetails

        [HttpGet]
        public async Task<IActionResult> QuestionDetail(int QuestionId)
        {
            var model = await questionService.GetQuestionDetail(QuestionId);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> QuestionDetail(QuestionDetailViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Answer))
            {
                await questionService.AddAnswerToQuestion(model);
                TempData[SuccessMessage] = "پاسخ شما ثبت شد";
                return RedirectToAction(nameof(QuestionList));
            }
       
          
            return View();
        }
        #endregion

        #region CloseQuestion

        public async Task<IActionResult> CloseQuestion(int questionId)
        {
            await questionService.CloseQuestion(questionId);
            return RedirectToAction(nameof(QuestionList));
        }

        #endregion

        #region ConfirmQuestion

        public async Task<IActionResult> ConfirmQuestion(int questionId)
        {
            await questionService.ConfirmToShow(questionId);
            return RedirectToAction(nameof(QuestionList));
        }

        #endregion

    }
}
