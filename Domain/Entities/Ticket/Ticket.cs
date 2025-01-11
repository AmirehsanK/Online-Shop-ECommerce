using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Account;
using Domain.Entities.Common;
using static Domain.Enums.TicketsEnum;

namespace Domain.Entities.Ticket;

public class Ticket : BaseEntity
{
    [ForeignKey("User")] public int OwnerId { get; set; }

    public string Title { get; set; }

    public DateTime UpdateDate { get; set; }

    public Section Section { get; set; }

    public Priority Priority { get; set; }

    public Status Status { get; set; }

    #region relation

    public ICollection<TicketsMessage> TicketsMessages { get; set; }
    public User User { get; set; }

    #endregion
}