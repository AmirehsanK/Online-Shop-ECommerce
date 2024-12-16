
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities.Notification
{
    public class Notification : BaseEntity
    {
        public string Message { get; set; }

        public int? UserId { get; set; }


        #region Relation

        public ICollection<UserNotification> UserNotification { get; set; }

        #endregion

   


    }
}
