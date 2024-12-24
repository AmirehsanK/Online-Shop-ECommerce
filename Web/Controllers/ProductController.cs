using Application.Services.Interfaces;
using Application.Tools;
using Domain.ViewModel.Product.Product;
using Domain.ViewModel.Question;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ProductController(IProductService productService,
    IQuestionService questionService) : SiteBaseController
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

        #endregion
   
    }


}
