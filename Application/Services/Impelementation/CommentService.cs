using System.Security.Claims;
using Application.Services.Interfaces;
using Application.Tools;
using Domain.Entities.Account;
using Domain.Entities.Comment;
using Domain.Entities.Question;
using Domain.Interface;
using Domain.ViewModel.Comment;
using Domain.ViewModel.Question;
using Infra.Data.Repositories;

namespace Application.Services.Impelementation
{
    public class CommentService(ICommentRepository commentRepository,IUserRepository userRepository) : ICommentService
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
                Innovation = viewModel.Innovation,
                BuildQuality = viewModel.BuildQuality,
                EaseOfUse = viewModel.EaseOfUse,
                DesignAndAppearance = viewModel.DesignAndAppearance,
                ValueForMoney = viewModel.ValueForMoney,
                Features = viewModel.Features,
                UserId = viewModel.UserId
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

        public async Task<List<CommentViewModel>> GetCommentsByProductIdAsync(int productId)
        {
            var comment = await commentRepository.GetCommentByProductIdAsync(productId);
            var list = new List<CommentViewModel>() ;
            foreach (var variable in comment)
            {
                list.Add(new CommentViewModel()
                {
                    CreateDate = variable.CreateDate,
                    Innovation = variable.Innovation,
                    BuildQuality = variable.BuildQuality,
                    EaseOfUse = variable.EaseOfUse,
                    Features = variable.Features,
                    ValueForMoney = variable.ValueForMoney,
                    DesignAndAppearance = variable.DesignAndAppearance,
                    Strengths = variable.Strengths,
                    Weaknesses = variable.Weaknesses,
                    Content = variable.Content,
                    Title = variable.Title,
                    IsApproved = variable.IsApproved,
                    Id = variable.Id,
                    ProductId = variable.ProductId
                    ,Likes = await commentRepository.GetCommentLikesByIdAsync(variable.Id),
                    Dislikes = await commentRepository.GetCommentDislikesByIdAsync(variable.Id)
                    ,UserId = variable.UserId,
                    UserName = await userRepository.GetUserNameByIdAsync(variable.Id)
                });
            }
            return list;
        }
    }
}
