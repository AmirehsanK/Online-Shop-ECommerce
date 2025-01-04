

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
        public TransactionStatus TransactionStatus { get; set; }

        #region Relation

        public User User { get; set; }

        #endregion
    }

    public enum TransactionType : byte
    {
        WithDraw,
        Deposit
    }

    public enum TransactionStatus
    {
        Confirm,
        NotConfirmed
    }
}
