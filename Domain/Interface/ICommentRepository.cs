using Domain.Entities.Comment;
using Domain.ViewModel.Comment;

namespace Domain.Interface;

public interface ICommentRepository
{
    #region Comment Interaction

    Task LikeCommentAsync(int commentId, string userIp, bool isLike);

    #endregion

    #region Comment Retrieval

    Task<List<Comment>> GetAllPendingCommentsAsync();
    Task<FilterCommentViewModel> GetAllCommentsAsync(FilterCommentViewModel filter);
    Task<List<Comment>> GetCommentByProductIdAsync(int productId);
    Task<int> GetCommentLikesByIdAsync(int commentId);
    Task<int> GetCommentDislikesByIdAsync(int commentId);
    Task<Comment> GetCommentByIdAsync(int id);
    Task<List<Comment>> GetCommentsByUserIdAsync(int userId);

    #endregion

    #region Comment Management

    Task AddCommentAsync(Comment comment);
    Task ApproveCommentAsync(int id);
    Task DeleteComment(Comment comment);

    #endregion
}