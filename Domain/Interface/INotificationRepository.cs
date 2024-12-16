
using Domain.Entities.Notification;
using Domain.Enums;

namespace Domain.Interface
{
    public interface INotificationRepository
    {
        Task AddUserNotificationAsync(UserNotification  userNotification);

        Task AddNotificationAsync(Notification  notification);

        Task SaveChangesAsync();

        Task<Notification> GetPrivateNotification(int userid);

        Task<Notification> GetPublicNotification(int userid);

    }
}
