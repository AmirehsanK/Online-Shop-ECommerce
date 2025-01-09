using Application.Services.Impelementation;
using Application.Services.Interfaces;
using Application.Tools;
using Azure.Core;
using Domain.Entities.Question;
using Domain.Enums;
using Domain.ViewModel.Comment;
using Domain.ViewModel.Product.Product;
using Domain.ViewModel.Question;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ProductController(IProductService productService,
    IQuestionService questionService,ICommentService commentService,IOrderService orderService) : SiteBaseController
     
    {
        
        #region ProductList

        [HttpGet("ProductList/")]
        public async Task<IActionResult> ProductList(FilterProductViewModel filter, int subCategoryId)
        {
            
            var model = await productService.GetAllProductsAsync(filter);
            return View(model);

        }


        #endregion

        #region Product Detail

        [HttpGet("product-detail")]
        public async Task<IActionResult> ProductDetail(int productId)
        {
            var comment = await commentService.GetCommentsByProductIdAsync(productId);
            ViewData["CommentAvg"] = await commentService.GetCommentRatingsAsync(productId);
            ViewData["Comments"] = comment;
            ViewData["Question"] = await questionService.GetProductQuestionsById(productId);
            var model= await productService.GetProductDetailForSite(productId);
            return View(model);
        }
        
        #endregion

        #region Add Question To Product
        
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

        #endregion

        #region Toggle Question Like

        [HttpPost]
        public async Task<IActionResult> ToggleQuestionLike(int productId, int userId, QuestionLike questionLike)
        {
            var isSuccess = await questionService.ToggleQuestionLike(productId, userId, questionLike);
            return Json(new { success = isSuccess });
        }
        #endregion

        #region Comments
        [HttpGet]
        public async Task<IActionResult> ProductAddComment(int productId)
        {
            var product = await productService.GetProductByIdAsync(productId);
            ViewData["ProductName"] = product.ProductName;
            ViewData["ProductImage"] = product.ImageName;
            var model = new CommentViewModel
            {
                ProductId = product.Id
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ProductAddComment(CommentViewModel comment)
        {
            comment.UserId=User.GetCurrentUserId();
            await commentService.AddCommentAsync(comment);
            TempData[SuccessMessage] = "نظر شما با موفقیت ثبت شد";
            return RedirectToAction(nameof(ProductDetail),new{ productid = comment.ProductId});
        }
        public async Task<IActionResult> CommentLike(int commentId, bool isLike)
        {
            var userIp = Request.HttpContext.Connection.RemoteIpAddress!.ToString();
            await commentService.LikeCommentAsync(commentId, userIp, isLike);
            return Json(new { success = true });
        }
        #endregion

        #region Add To Basket
      
        

        #endregion
        
    }


}
