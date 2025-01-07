using System.ComponentModel.DataAnnotations;
using Domain.Entities.Common;
using Domain.Entities.Discount;
using Domain.Entities.Notification;
using Domain.Entities.Orders;
using Domain.Entities.Permission;
using Domain.Entities.Question;
using Domain.Entities.Ticket;

namespace Domain.Entities.Account;

public class User : BaseEntity
{
    [MaxLength(200)] 
    public string? FirstName { get; set; }

    [MaxLength(200)]
    public string? LastName { get; set; }

    [MaxLength(200)]
    public string Email { get; set; }

    [MaxLength(11)]
    public string PhoneNumber { get; set; }

    [MaxLength(200)]
    public string Password { get; set; }
    
    public string? Address { get; set; }

    public string EmailActiveCode { get; set; }

    public bool IsAdmin { get; set; }
    public bool IsEmailActive { get; set; }

    #region Relation

    public ICollection<Ticket.Ticket> Ticket { get; set; }

    public ICollection<TicketsMessage> TicketsMessage { get; set; }

    public ICollection<UserNotification> UserNotification { get; set; }

    public ICollection<UserDiscount> UserDiscount { get; set; }

    public ICollection<QuestionAnswer> QuestionAnswers { get; set; }

    public ICollection<Order> Orders { get; set; }

    public ICollection<Transaction> Transactions { get; set; }
    
    public ICollection<UserRoleMapping> UserRoleMappings { get; set; }

    #endregion


}