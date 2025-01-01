using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Account;
using Domain.Entities.Common;

namespace Domain.Entities.Comment
{
    public class Comment : BaseEntity
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public string? Strengths { get; set; }
        public string? Weaknesses { get; set; }
        public bool IsApproved { get; set; }
        public int BuildQuality { get; set; }
        public int ValueForMoney { get; set; }
        public int Innovation { get; set; }
        public int Features { get; set; }
        public int EaseOfUse { get; set; }
        public int DesignAndAppearance { get; set; }

        #region Relations

        [ForeignKey(nameof(ProductId))]
        public ICollection<Product.Product> Product { get; set; }
        [ForeignKey(nameof(UserId))]
        public ICollection<User> User { get; set; }
        public ICollection<UserInteraction> Interactions { get; set; }

        #endregion

    }
}
