using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class SiteFaqController : SiteBaseController
    {
        #region Ctor

        private readonly IFaqService _faqService;

        public SiteFaqController(IFaqService faqService)
        {
            _faqService = faqService;
        }

        #endregion

        #region FAq Category

        [HttpGet("Faq")]
        public async Task<IActionResult> Faq()
        {
            var model = await _faqService.GetAllCategoryForSite();
            return View(model);
        }

        #endregion


        [HttpGet("Faq-Detail")]
        public async Task<IActionResult> FaqCategoryDetail(int categoryid)
        {
           var model= await _faqService.GetFaqCategories(categoryid);
            return View(model);
        }

        //[HttpGet("Faq")]
        //public IActionResult Faq()
        //{
        //    return View();
        //}

    }
}
