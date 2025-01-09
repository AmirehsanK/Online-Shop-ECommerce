using Domain.Entities.Notification;

namespace Domain.Interface;

public interface INotificationRepository
{

    #region Notification Management

    Task AddUserNotificationAsync(UserNotification userNotification);
    Task AddNotificationAsync(Notification notification);
    Task<Notification> GetPrivateNotification(int userid);
    Task<Notification> GetPublicNotification(int userid);

    #endregion

    #region Save Changes

    Task SaveChangesAsync();

    #endregion

}