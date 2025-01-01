using Domain.ViewModel.Comment;

namespace Application.Services.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentViewModel>> GetPendingCommentsAsync();
        Task<FilterCommentViewModel> GetCommentsAsync(FilterCommentViewModel filter);
        Task<List<CommentViewModel>> GetCommentsByProductIdAsync(int productId);
        Task ApproveCommentAsync(int id);
        Task AddCommentAsync(CommentViewModel viewModel);
        Task LikeCommentAsync(int commentId, string userIp, bool isLike);
        Task DeleteComment(int commentId);
    }
}
