﻿using Application.Services.Interfaces;
using Domain.ViewModel.Comment;
using Infra.Data.Statics;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;

namespace Web.Areas.Admin.Controllers;

[InvokePermission(PermissionName.CommentManagement)]
public class CommentController(ICommentService commentService) : AdminBaseController
{
    #region Comment List

    [InvokePermission(PermissionName.CommentList)]
    public async Task<IActionResult> CommentList(FilterCommentViewModel filters, string filterModel)
    {
        filters.Filter = filterModel;
        var comments = await commentService.GetCommentsAsync(filters);
        return View(comments);
    }

    #endregion

    #region Approve Comment

    [HttpPost]
    [InvokePermission(PermissionName.DeleteComment)]
    public async Task<IActionResult> Approve(int id)
    {
        await commentService.ApproveCommentAsync(id);
        return RedirectToAction(nameof(CommentList));
    }

    #endregion

    #region Delete Comment

    [HttpPost]
    [InvokePermission(PermissionName.DeleteComment)]
    public async Task<IActionResult> Delete(int id)
    {
        await commentService.DeleteComment(id);
        return RedirectToAction(nameof(CommentList));
    }

    #endregion
}