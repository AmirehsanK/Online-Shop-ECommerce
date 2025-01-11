using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents.ContactUsViewComponent;

public class ContactUsSubjectViewComponent : ViewComponent
{
    private readonly IContactUsService _contactUsService;

    public ContactUsSubjectViewComponent(IContactUsService contactUsService)
    {
        _contactUsService = contactUsService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var subjects = await _contactUsService.GetSubjectsAsync();
        return View(subjects);
    }
}