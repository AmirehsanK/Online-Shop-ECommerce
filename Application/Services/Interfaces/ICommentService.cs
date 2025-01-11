using Domain.ViewModel.Comment;

namespace Application.Services.Interfaces;

public interface ICommentService
{
    #region Comment Interaction

    Task LikeCommentAsync(int commentId, string userIp, bool isLike);

    #endregion

    #region Comment Retrieval

    Task<List<CommentViewModel>> GetPendingCommentsAsync();
    Task<FilterCommentViewModel> GetCommentsAsync(FilterCommentViewModel filter);
    Task<List<CommentViewModel>> GetCommentsByProductIdAsync(int productId);
    Task<Dictionary<string, float>> GetCommentRatingsAsync(int productId);
    Task<List<CommentViewModel>> GetCommentsByUserIdAsync(int userId);

    #endregion

    #region Comment Management

    Task ApproveCommentAsync(int id);
    Task AddCommentAsync(CommentViewModel viewModel);
    Task DeleteComment(int commentId);

    #endregion
}