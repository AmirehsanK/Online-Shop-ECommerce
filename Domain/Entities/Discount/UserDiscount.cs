using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Account;

namespace Domain.Entities.Discount;

public class UserDiscount
{
    public int Id { get; set; }

    [ForeignKey(nameof(User))] public int UserId { get; set; }

    [ForeignKey(nameof(Discount))] public int DiscountId { get; set; }

    #region Relations

    public Discount Discount { get; set; }
    public User User { get; set; }

    #endregion
}