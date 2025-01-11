using Domain.Entities.Notification;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class NotificationRepository(ApplicationDbContext context) : INotificationRepository
{

    #region Add Methods

    public async Task AddUserNotificationAsync(UserNotification userNotification)
    {
        await context.UserNotifications.AddAsync(userNotification);
    }

    public async Task AddNotificationAsync(Notification notification)
    {
        await context.Notifications.AddAsync(notification);
    }

    #endregion

    #region Save Changes

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    #endregion

    #region Get Methods

    public async Task<Notification> GetPrivateNotification(int userid)
    {
        return (await context.Notifications
            .Where(u => u.UserId == userid && u.UserNotification.All(s => s.UserId != userid))
            .FirstOrDefaultAsync())!;
    }

    public async Task<Notification> GetPublicNotification(int userid)
    {
        return (await context.Notifications
            .Where(u => u.UserId == null && u.UserNotification.All(s => s.UserId != userid))
            .FirstOrDefaultAsync())!;
    }

    #endregion

}