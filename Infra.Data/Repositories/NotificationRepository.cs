
using Domain.Entities.Notification;
using Domain.Enums;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories
{
    public class NotificationRepository:INotificationRepository
    {
        #region Ctor

        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task AddUserNotificationAsync(UserNotification userNotification)
        {
           await _context.UserNotifications.AddAsync(userNotification);
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Notification> GetPrivateNotification(int userid)
        {
            return await _context.Notifications.Where(u => u.UserId == userid && u.UserNotification.All(s => s.UserId != userid)).FirstOrDefaultAsync();
        }
        public async Task<Notification> GetPublicNotification(int userid)
        {
            return await _context.Notifications.Where(u=> u.UserId == null&& u.UserNotification.All(s => s.UserId != userid)).FirstOrDefaultAsync();
        }
    }
}
