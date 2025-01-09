using Application.Services.Impelementation;
using Application.Services.Interfaces;
using Domain.ViewModel.Comment;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;
using Infra.Data.Statics;

namespace Web.Areas.Admin.Controllers
{
    [InvokePermission(PermissionName.CommentManagement)]
    public class CommentController(ICommentService commentService) : AdminBaseController
    {
        #region Comment List

        [InvokePermission(PermissionName.CommentList)]
        public async Task<IActionResult> CommentList(FilterCommentViewModel filter)
        {
            var comments = await commentService.GetCommentsAsync(filter);
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
}