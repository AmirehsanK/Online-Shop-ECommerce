
using Application.Services.Interfaces;
using Domain.Entities.Notification;
using Domain.Enums;
using Domain.Interface;
using Domain.ViewModel.Notification;

namespace Application.Services.Impelementation
{
    public class NotificationService : INotificationService
    {
        #region Ctor

        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        #endregion

        public async Task AddNewMessage(AddNotificationViewModel model, int? userId)
        {
            var newnotification = new Notification()
            {
                CreateDate = DateTime.Now,
                IsDeleted = false,
                Message = model.Message,
            };
            if (userId.HasValue)
            {
                newnotification.UserId = userId.Value;
            }

            await _notificationRepository.AddNotificationAsync(newnotification);

            await _notificationRepository.SaveChangesAsync();
        }

        //public async Task<NotificationEnum> GetNotificationById(int userid)
        //{
        //    var message = await _notificationRepository.GetNotification(userid);
        //    if (message.Where(u => u.UserId == userid)!=null)
        //    {
        //        return NotificationEnum.HasMessage;
        //    }
        //    else if(message.Where(u => u.UserId == null)!=null)
        //    {
        //        return NotificationEnum.PublicMessage;
        //    }

        //    return NotificationEnum.NotFound;

        //}

        public async Task<ShowNotificationViewModel> GetShowNotificationById(int userid)
        {
            var privatemessage = await _notificationRepository.GetPrivateNotification(userid);
            if(privatemessage!=null)
            {
                var notification = new ShowNotificationViewModel()
                {
            
                    Message = privatemessage.Message,
                    MessageId = privatemessage.Id,
                    userid= privatemessage.UserId.Value

          
                };
                return notification;
            }

            return null;





        }

        public async Task markSeenForPrivateMessage(int? userId, int message)
        {
            var seen = new UserNotification()
            {
                CreateDate = DateTime.Now,
                NotificationId = message,
                UserId = userId.Value,
                IsDeleted = false
            };
            await _notificationRepository.AddUserNotificationAsync(seen);
            await _notificationRepository.SaveChangesAsync();
        }

        public async Task<NotificationEnum> GetNotificationById(int id)
        {
            var message = await _notificationRepository.GetPrivateNotification(id);
     
            if (message==null)
            {
                return NotificationEnum.NotFound;
            }
            else
            {
                return NotificationEnum.HasMessage;
            }

        }

        public async Task<Notification> GetpublicMessage(int userId)
        {
            var message = await _notificationRepository.GetPublicNotification(userId);
            if (message!=null)
            {
                return message;
            }

            return null;

        }
    }
}
