using Application.Services.Interfaces;
using Domain.Entities.Comment;
using Domain.Interface;
using Domain.ViewModel.Comment;

namespace Application.Services.Impelementation
{
    public class CommentService(ICommentRepository commentRepository) : ICommentService
    {

        public async Task<List<CommentViewModel>> GetPendingCommentsAsync()
        {
            var comments = await commentRepository.GetAllPendingCommentsAsync();
            return comments.Select(c => new CommentViewModel
            {
                Id = c.Id,
                Title = c.Title,
                Content = c.Content,
                Strengths = c.Strengths!,
                Weaknesses = c.Weaknesses!,
                CreateDate = c.CreateDate,
                IsApproved = c.IsApproved,
                Ratings = c.Ratings.Select(r => new CommentRatingViewModel
                {
                    Category = r.Category,
                    Score = r.Score
                }).ToList(),
                Likes = c.Interactions.Count(i => i.IsLike),
                Dislikes = c.Interactions.Count(i => !i.IsLike)
            }).ToList();
        }
        public async Task<FilterCommentViewModel> GetCommentsAsync(FilterCommentViewModel filter)
        {
            return await commentRepository.GetAllCommentsAsync(filter);
        }
        public async Task ApproveCommentAsync(int id)
        {
            await commentRepository.ApproveCommentAsync(id);
        }

        public async Task AddCommentAsync(CommentViewModel viewModel)
        {
            var comment = new Comment
            {
                ProductId = viewModel.ProductId,
                Title = viewModel.Title,
                Content = viewModel.Content,
                Strengths = viewModel.Strengths,
                Weaknesses = viewModel.Weaknesses,
                IsApproved = false,
                CreateDate = DateTime.UtcNow,
                Ratings = viewModel.Ratings.Select(r => new CommentRating
                    {
                        Category = r.Category,
                        Score = r.Score,
                        Comment = null
                    })
                    .ToList(),
                Product = null
            };

            await commentRepository.AddCommentAsync(comment);
        }

        public async Task LikeCommentAsync(int commentId, string userIp, bool isLike)
        {
            await commentRepository.LikeCommentAsync(commentId, userIp, isLike);
        }

        public async Task DeleteComment(int commentId)
        {
            var comment = await commentRepository.GetCommentByIdAsync(commentId);
            comment.IsDeleted=true;
            await commentRepository.DeleteComment(comment);
        }

    }
}
