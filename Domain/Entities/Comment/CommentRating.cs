using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Common;

namespace Domain.Entities.Comment
{
    public class CommentRating:BaseEntity
    {
       
        public int CommentId { get; set; }
        public RatingCategory Category { get; set; }
        public int Score { get; set; }

        [ForeignKey(nameof(CommentId))]
        public required Comment Comment { get; set; }
    }
}
