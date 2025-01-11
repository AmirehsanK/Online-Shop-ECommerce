using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Account;
using Domain.Entities.Common;

namespace Domain.Entities.Orders;

public class Order : BaseEntity
{
    #region Proprties

    [ForeignKey(nameof(User))] public int UserId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public bool IsFinally { get; set; }


    public string? RefCode { get; set; }

    #endregion

    #region Relation

    public User User { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; }

    #endregion
}