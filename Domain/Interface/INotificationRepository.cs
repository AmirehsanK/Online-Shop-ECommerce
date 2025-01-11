using Domain.Entities.Notification;

namespace Domain.Interface;

public interface INotificationRepository
{
    #region Save Changes

    Task SaveChangesAsync();

    #endregion

    #region Notification Management

    Task AddUserNotificationAsync(UserNotification userNotification);
    Task AddNotificationAsync(Notification notification);
    Task<Notification> GetPrivateNotification(int userid);
    Task<Notification> GetPublicNotification(int userid);

    #endregion
}