using Application.Services.Interfaces;
using Domain.Entities.Notification;
using Domain.Enums;
using Domain.Interface;
using Domain.ViewModel.Notification;

namespace Application.Services.Impelementation;

public class NotificationService(INotificationRepository notificationRepository) : INotificationService
{
    #region Add Notification

    public async Task AddNewMessage(AddNotificationViewModel model, int? userId)
    {
        var newNotification = new Notification
        {
            CreateDate = DateTime.Now,
            IsDeleted = false,
            Message = model.Message
        };
        if (userId.HasValue) newNotification.UserId = userId.Value;
        await notificationRepository.AddNotificationAsync(newNotification);
        await notificationRepository.SaveChangesAsync();
    }

    #endregion

    #region Notification Status

    public async Task<NotificationEnum> GetNotificationById(int id)
    {
        var message = await notificationRepository.GetPrivateNotification(id);
        return message == null! ? NotificationEnum.NotFound : NotificationEnum.HasMessage;
    }

    #endregion

    #region Mark Notification as Seen

    public async Task markSeenForPrivateMessage(int? userId, int message)
    {
        var seen = new UserNotification
        {
            CreateDate = DateTime.Now,
            NotificationId = message,
            UserId = userId!.Value,
            IsDeleted = false
        };
        await notificationRepository.AddUserNotificationAsync(seen);
        await notificationRepository.SaveChangesAsync();
    }

    #endregion

    #region Get Notifications

    public async Task<ShowNotificationViewModel> GetShowNotificationById(int userid)
    {
        var privateMessage = await notificationRepository.GetPrivateNotification(userid);
        if (privateMessage == null!) return null!;
        var notification = new ShowNotificationViewModel
        {
            Message = privateMessage.Message,
            MessageId = privateMessage.Id,
            userid = privateMessage.UserId!.Value
        };
        return notification;
    }

    public async Task<Notification> GetpublicMessage(int userId)
    {
        var message = await notificationRepository.GetPublicNotification(userId);
        return message != null! ? message : null!;
    }

    #endregion
}