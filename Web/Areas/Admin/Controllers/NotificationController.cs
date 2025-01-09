using Application.Services.Interfaces;
using Domain.ViewModel.Notification;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;
using Infra.Data.Statics;

namespace Web.Areas.Admin.Controllers
{
    [InvokePermission(PermissionName.NotificationManagement)]
    public class NotificationController(INotificationService notificationService) : AdminBaseController
    {
        
        #region Add New Notification

        [HttpGet]
        [InvokePermission(PermissionName.CreateNotification)]
        public IActionResult AddNewNotification(int? userId)
        {
            if (userId.HasValue)
            {
                ViewData["Id"] = userId.Value;
            }
            return View();
        }

        [HttpPost]
        [InvokePermission(PermissionName.CreateNotification)]
        public async Task<IActionResult> AddNewNotification(AddNotificationViewModel model, int? userId = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await notificationService.AddNewMessage(model, userId);
            TempData[SuccessMessage] = "اعلان با موفقیت ارسال شد.";
            return RedirectToAction(nameof(AddNewNotification));
        }
        
        #endregion
        
    }
}