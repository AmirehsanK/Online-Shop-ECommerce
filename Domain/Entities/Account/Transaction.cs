

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities.Account
{
    public class Transaction:BaseEntity
    {
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public int Price { get; set; }

        public TransactionType TransactionType { get; set; }

        public bool IsPay { get; set; }


        #region Relation

        public User User { get; set; }

        #endregion
    }

    public enum TransactionType : byte
    {
        [Display(Name = "برداشت")]
        WithDraw,
        [Display(Name = "واریز")]
        Deposit
    }

 
}
