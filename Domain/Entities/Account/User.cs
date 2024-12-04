using System.ComponentModel.DataAnnotations;
using Domain.Entities.Common;
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

    [MaxLength(200)]
    public string Password { get; set; }

    public string EmailActiveCode { get; set; }

    public bool IsAdmin { get; set; }
    public bool IsEmailActive { get; set; }

    public ICollection<Ticket.Ticket> Ticket { get; set; }

    public ICollection<TicketsMessage> TicketsMessage { get; set; }
}