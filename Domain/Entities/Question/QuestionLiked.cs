using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Account;
using Domain.Entities.Common;

namespace Domain.Entities.Question;

public class QuestionLiked : BaseEntity
{
    [ForeignKey(nameof(Product))] public int ProductId { get; set; }

    [ForeignKey(nameof(User))] public int UserId { get; set; }


    public QuestionLike QuestionLike { get; set; }

    #region Relation

    public Product.Product Product { get; set; }

    public User User { get; set; }

    #endregion
}

public enum QuestionLike
{
    Liked,
    Disliked,
    None
}