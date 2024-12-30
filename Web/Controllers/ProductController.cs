using Application.Services.Interfaces;
using Application.Tools;
using Domain.Entities.Question;
using Domain.Enums;
using Domain.ViewModel.Comment;
using Domain.ViewModel.Product.Product;
using Domain.ViewModel.Question;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ProductController(IProductService productService,
    IQuestionService questionService,ICommentService commentService) : SiteBaseController
    {
        #region ProductList

        [HttpGet("ProductList/")]
        public async Task<IActionResult> ProductList(FilterProductViewModel filter, int SubCategoryId)
        {
            
            var model = await productService.GetAllProductsAsync(filter);
            return View(model);

        }


        #endregion

        #region ProductDetail
        [HttpGet("ProductDetail/{productid}")]
        public async Task<IActionResult> ProductDetail(int productid)
        {
            ViewData["Question"] = await questionService.GetProductQuestionsById(productid);
           var model= await productService.GetProductDetailForSite(productid);
            return View(model);
        }
        
        #endregion

        #region AddQuestionToProduct

      
        [HttpPost]
        public async Task<IActionResult> AddQuestionToProduct(QuestionAnswerViewModel model)
        {
            #region Validation

            if (!ModelState.IsValid)
            {
                ViewBag.referer = HttpContext.Request.Headers.Referer;
                TempData[ErrorMessage] = "سوال شما وارد نشد";
                return Redirect(ViewBag.referer);
            }

            #endregion
            
            await questionService.AddNewQuestionToProduct(model, User.GetCurrentUserId());
            ViewBag.referer = HttpContext.Request.Headers.Referer;
            TempData[SuccessMessage] = "سوال شما با موفیقیت ثبت شد پس از تایید پاسخ داده میشود";
            return Redirect(ViewBag.referer);
        }
        [HttpPost]
        public async Task<IActionResult> ToggleQuestionLike(int productId, int userId, QuestionLike questionLike)
        {
            bool isSuccess = await questionService.ToggleQuestionLike(productId, userId, questionLike);
            return Json(new { success = isSuccess });
        }
        #endregion

        #region Comments
        [HttpGet]
        public async Task<IActionResult> ProductAddComment(int productId)
        {
            var model = new CommentViewModel
            {
                Ratings = Enum.GetValues(typeof(RatingCategory))
                    .Cast<RatingCategory>()
                    .Select(category => new CommentRatingViewModel { Category = category })
                    .ToList()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ProductAddComment(CommentViewModel comment)
        {
            await commentService.AddCommentAsync(comment);
            TempData[SuccessMessage] = "نظر شما با موفقیت ثبت شد";
            return RedirectToAction(nameof(ProductDetail),new{ productid = comment.ProductId});
        }

        #endregion
    }


}
