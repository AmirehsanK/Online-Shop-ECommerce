using Application.Services.Impelementation;
using Application.Services.Interfaces;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents.ContactUsViewComponent;

public class ContactUsSubjectViewComponent(ContactUsService contactUsService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var subjects = await contactUsService.GetSubjectsAsync();
        return View(subjects);
    }
}
