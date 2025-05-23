﻿using Application.Services.Interfaces;
using Domain.ViewModel.Question;
using Infra.Data.Statics;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;

namespace Web.Areas.Admin.Controllers;

[InvokePermission(PermissionName.QuestionManagement)]
public class QuestionController(IQuestionService questionService) : AdminBaseController
{
    #region QuestionList

    [InvokePermission(PermissionName.QuestionList)]
    public async Task<IActionResult> QuestionList(FilterQuestionListViewModel filter)
    {
        var model = await questionService.GetQuestion(filter);
        return View(model);
    }

    #endregion

    #region CloseQuestion

    [InvokePermission(PermissionName.QuestionList)]
    public async Task<IActionResult> CloseQuestion(int questionId)
    {
        await questionService.CloseQuestion(questionId);
        return RedirectToAction(nameof(QuestionList));
    }

    #endregion

    #region ConfirmQuestion

    [InvokePermission(PermissionName.QuestionList)]
    public async Task<IActionResult> ConfirmQuestion(int questionId)
    {
        await questionService.ConfirmToShow(questionId);
        return RedirectToAction(nameof(QuestionList));
    }

    #endregion

    #region QuestionDetails

    [HttpGet]
    [InvokePermission(PermissionName.QuestionList)]
    public async Task<IActionResult> QuestionDetail(int questionId)
    {
        var model = await questionService.GetQuestionDetail(questionId);
        return View(model);
    }

    [HttpPost]
    [InvokePermission(PermissionName.AnswerQuestion)]
    public async Task<IActionResult> QuestionDetail(QuestionDetailViewModel model)
    {
        if (string.IsNullOrEmpty(model.Answer)) return View();
        await questionService.AddAnswerToQuestion(model);
        TempData[SuccessMessage] = "پاسخ شما ثبت شد";
        return RedirectToAction(nameof(QuestionList));
    }

    #endregion
}