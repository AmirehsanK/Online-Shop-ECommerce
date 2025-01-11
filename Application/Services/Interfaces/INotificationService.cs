using Domain.Entities.Notification;
using Domain.Enums;
using Domain.ViewModel.Notification;

namespace Application.Services.Interfaces;

public interface INotificationService
{
    #region Notification Management

    Task AddNewMessage(AddNotificationViewModel model, int? userId);
    Task<ShowNotificationViewModel> GetShowNotificationById(int userid);
    Task markSeenForPrivateMessage(int? userId, int message);
    Task<NotificationEnum> GetNotificationById(int id);
    Task<Notification> GetpublicMessage(int userId);

    #endregion
}