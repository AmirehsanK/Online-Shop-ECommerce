using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class SiteFaqController : SiteBaseController
{
    #region Faq Category

    [HttpGet("Faq")]
    public async Task<IActionResult> Faq()
    {
        var model = await _faqService.GetAllCategoryForSite();
        return View(model);
    }

    #endregion

    #region Faq Category Detail

    [HttpGet("Faq-Detail")]
    public async Task<IActionResult> FaqCategoryDetail(int categoryId)
    {
        var model = await _faqService.GetFaqCategories(categoryId);
        return View(model);
    }

    #endregion

    #region Ctor

    private readonly IFaqService _faqService;

    public SiteFaqController(IFaqService faqService)
    {
        _faqService = faqService;
    }

    #endregion
}