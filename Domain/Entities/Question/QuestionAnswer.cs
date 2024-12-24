

using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Account;
using Domain.Entities.Common;

namespace Domain.Entities.Question
{
    public class QuestionAnswer : BaseEntity
    {
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        [ForeignKey(nameof(UserId))]
        public int UserId { get; set; }
        public string Question { get; set; }

        public string? Answer { get; set; }



        public QuestionStatus QuestionAnswers { get; set; }

        #region Relation

        public Product.Product Product { get; set; }
        public User User { get; set; }


        #endregion



    }

    public enum QuestionStatus
    {
        Answered,
        NotConfirmed,
        Confirmed,
        IsClosed
    }

}
