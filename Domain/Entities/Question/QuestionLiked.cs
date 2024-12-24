
using Domain.Entities.Common;

namespace Domain.Entities.Question
{
    public class QuestionLiked:BaseEntity
    {
        public int ProductId { get; set; }

        public int  UserId { get; set; }

        public string UserIp { get; set; }

        public QuestionLike QuestionLike { get; set; }
    
    }
    public enum QuestionLike
    {
        Liked,
        Disliked,
        None
    }
}
