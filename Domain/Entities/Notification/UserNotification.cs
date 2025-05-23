﻿using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Account;
using Domain.Entities.Common;

namespace Domain.Entities.Notification;

public class UserNotification : BaseEntity
{
    [ForeignKey(nameof(User))] public int UserId { get; set; }

    [ForeignKey(nameof(Notification))] public int NotificationId { get; set; }


    #region Relation

    public User User { get; set; }

    public Notification Notification { get; set; }

    #endregion
}