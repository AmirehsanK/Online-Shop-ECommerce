using Domain.Entities.Comment;
using Domain.ViewModel.Comment;

namespace Domain.Interface
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllPendingCommentsAsync();
        Task<FilterCommentViewModel> GetAllCommentsAsync(FilterCommentViewModel filter);

        Task<Comment> GetCommentByIdAsync(int id);
        Task AddCommentAsync(Comment comment);
        Task ApproveCommentAsync(int id);
        Task LikeCommentAsync(int commentId, string userIp, bool isLike);
        Task DeleteComment(Comment comment);
    }
}
