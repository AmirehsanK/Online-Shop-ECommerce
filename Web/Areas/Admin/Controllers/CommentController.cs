using Application.Services.Impelementation;
using Application.Services.Interfaces;
using Domain.ViewModel.Comment;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class CommentController(ICommentService commentService) : AdminBaseController
    {
        public async Task<IActionResult> CommentList(FilterCommentViewModel filter)
        {

            var comments =await commentService.GetCommentsAsync(filter);

            return View(comments);
        }


        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
           await commentService.ApproveCommentAsync(id);
           return RedirectToAction(nameof(CommentList));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await commentService.DeleteComment(id);
            return RedirectToAction(nameof(CommentList));
        }
    }
}
