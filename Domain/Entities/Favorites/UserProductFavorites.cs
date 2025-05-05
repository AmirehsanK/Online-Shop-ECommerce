using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Account;
using Domain.Entities.Common;

namespace Domain.Entities.Favorites;

public class UserProductFavorites : BaseEntity
{
    #region Properties

    public int UserId { get; set; }
    public int ProductId { get; set; }

    #endregion

    #region Relations

    [ForeignKey(nameof(UserId))] public User User { get; set; }

    [ForeignKey(nameof(ProductId))] public Product.Product Product { get; set; }

    #endregion
}