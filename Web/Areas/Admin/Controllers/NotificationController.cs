using Application.Services.Interfaces;
using Domain.ViewModel.Notification;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class NotificationController : AdminBaseController
    {
        #region Ctor

        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        #endregion
        [HttpGet]
        public IActionResult AddNewNotification(int? userId)
        {
            if (userId.HasValue)
            {
                ViewData["Id"]=userId.Value;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddNewNotification(AddNotificationViewModel model,int? userId=null)
        {
            #region Validation
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            #endregion

            await _notificationService.AddNewMessage(model, userId);
            return View();
        }
    }
}
