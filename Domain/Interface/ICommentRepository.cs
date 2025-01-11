using Domain.Entities.Comment;
using Domain.ViewModel.Comment;

namespace Domain.Interface;

public interface ICommentRepository
{

    #region Comment Retrieval

    Task<List<Comment>> GetAllPendingCommentsAsync();
    Task<FilterCommentViewModel> GetAllCommentsAsync(FilterCommentViewModel filter);
    Task<List<Comment>> GetCommentByProductIdAsync(int productId);
    Task<int> GetCommentLikesByIdAsync(int commentId);
    Task<int> GetCommentDislikesByIdAsync(int commentId);
    Task<Comment> GetCommentByIdAsync(int id);

    #endregion

    #region Comment Management

    Task AddCommentAsync(Comment comment);
    Task ApproveCommentAsync(int id);
    Task DeleteComment(Comment comment);

    #endregion

    #region Comment Interaction

    Task LikeCommentAsync(int commentId, string userIp, bool isLike);

    #endregion

}