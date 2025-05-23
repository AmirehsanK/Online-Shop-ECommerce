﻿using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Account;
using Domain.Entities.Common;

namespace Domain.Entities.Ticket;

public class TicketsMessage : BaseEntity
{
    [ForeignKey("Ticket")] public int TicketId { get; set; }

    public string Text { get; set; }

    [ForeignKey("User")] public int SenderId { get; set; }

    public string? FileName { get; set; }

    #region Relation

    public Ticket? Ticket { get; set; }

    public User? User { get; set; }

    #endregion
}