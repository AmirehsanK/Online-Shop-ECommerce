using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities.Comment;

public class UserInteraction : BaseEntity
{
    public int CommentId { get; set; }

    [MaxLength(30)] public required string UserIp { get; set; }

    public required bool IsLike { get; set; }

    [ForeignKey(nameof(CommentId))] public required Comment Comment { get; set; }
}