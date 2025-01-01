using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Common;

namespace Domain.Entities.Comment
{
    public class Comment : BaseEntity
    {
        public int ProductId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public string? Strengths { get; set; }
        public string? Weaknesses { get; set; }
        public bool IsApproved { get; set; }

        #region Relations

        [ForeignKey(nameof(ProductId))]
        public required Product.Product Product { get; set; }
        public ICollection<CommentRating> Ratings { get; set; }
        public ICollection<UserInteraction> Interactions { get; set; }

        #endregion

    }
}
