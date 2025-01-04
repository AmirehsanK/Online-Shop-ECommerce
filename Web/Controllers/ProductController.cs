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
        public async Task<IActionResult> ProductList(FilterProductViewModel filter, int SubCategoryId)
        {
            
            var model = await productService.GetAllProductsAsync(filter);
            return View(model);

        }


        #endregion

        [HttpGet("product-detail")]
        public async Task<IActionResult> ProductDetail(int productid)
        {
            #region averageRating

            var comment = await commentService.GetCommentsByProductIdAsync(productid);
            float avgQuality = 0;   
            float avgDesign = 0;
            float avgEase = 0;
            float avgValue = 0;
            float avgInnovation = 0;
            float avgFeatures = 0;

            foreach (var variable in comment)
            {
                avgQuality += variable.BuildQuality;
                avgDesign += variable.DesignAndAppearance;
                avgEase += variable.EaseOfUse;
                avgValue += variable.ValueForMoney;
                avgInnovation += variable.Innovation;
                avgFeatures += variable.Features;
            }

            avgQuality /= comment.Count;
            avgDesign /= comment.Count;
            avgEase /= comment.Count;
            avgValue /= comment.Count;
            avgInnovation /= comment.Count;
            avgFeatures /= comment.Count;
            float avgOverall = (avgQuality + avgDesign + avgEase + avgValue + avgInnovation + avgFeatures) / 6;
            Dictionary<string, float> avg = new Dictionary<string, float>();
            avg.Add("avgQuality", avgQuality);
            avg.Add("avgDesign", avgDesign);
            avg.Add("avgEase", avgEase);
            avg.Add("avgValue", avgValue);
            avg.Add("avgInnovation", avgInnovation);
            avg.Add("avgFeatures", avgFeatures);
            avg.Add("avgOverall", avgOverall);
            ViewData["Commentavg"] = avg;
            #endregion
            ViewData["Comments"] = comment;
            ViewData["Question"] = await questionService.GetProductQuestionsById(productid);
            var model= await productService.GetProductDetailForSite(productid);
            return View(model);
        }
        
   

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

        #endregion

        #region MyRegion

        

     
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
            var product = await productService.GetProductByIdAsync(productId);
            ViewData["ProductName"] = product.ProductName;
            ViewData["ProductImage"] = product.ImageName;
            return View();
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
            var userIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            await commentService.LikeCommentAsync(commentId, userIp, isLike);
            return Json(new { success = true });
        }
        #endregion

        #region AddToBasket
      



        #endregion

 
    }


}
