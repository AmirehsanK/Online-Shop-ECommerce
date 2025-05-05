using Application.Services.Interfaces;
using Domain.Entities.Comment;
using Domain.Interface;
using Domain.ViewModel.Comment;

namespace Application.Services.Impelementation;

public class CommentService(ICommentRepository commentRepository, IUserRepository userRepository) : ICommentService
{
    #region Pending Comments

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

    #endregion

    #region Filter Comments

    public async Task<FilterCommentViewModel> GetCommentsAsync(FilterCommentViewModel filter)
    {
        return await commentRepository.GetAllCommentsAsync(filter);
    }

    #endregion

    #region Comment Approval

    public async Task ApproveCommentAsync(int id)
    {
        await commentRepository.ApproveCommentAsync(id);
    }

    #endregion

    #region Add Comment

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

    #endregion

    #region Like/Comment Interaction

    public async Task LikeCommentAsync(int commentId, string userIp, bool isLike)
    {
        await commentRepository.LikeCommentAsync(commentId, userIp, isLike);
    }

    #endregion

    #region Delete Comment

    public async Task DeleteComment(int commentId)
    {
        var comment = await commentRepository.GetCommentByIdAsync(commentId);
        comment.IsDeleted = true;
        await commentRepository.DeleteComment(comment);
    }

    #endregion

    #region Comments by Product ID

    public async Task<List<CommentViewModel>> GetCommentsByProductIdAsync(int productId)
    {
        var comment = await commentRepository.GetCommentByProductIdAsync(productId);
        var list = new List<CommentViewModel>();
        foreach (var variable in comment)
            list.Add(new CommentViewModel
            {
                CreateDate = variable.CreateDate,
                Innovation = variable.Innovation,
                BuildQuality = variable.BuildQuality,
                EaseOfUse = variable.EaseOfUse,
                Features = variable.Features,
                ValueForMoney = variable.ValueForMoney,
                DesignAndAppearance = variable.DesignAndAppearance,
                Strengths = variable.Strengths!,
                Weaknesses = variable.Weaknesses!,
                Content = variable.Content,
                Title = variable.Title,
                IsApproved = variable.IsApproved,
                Id = variable.Id,
                ProductId = variable.ProductId,
                Likes = await commentRepository.GetCommentLikesByIdAsync(variable.Id),
                Dislikes = await commentRepository.GetCommentDislikesByIdAsync(variable.Id),
                UserId = variable.UserId,
                UserName = await userRepository.GetUserNameByIdAsync(variable.Id)
            });
        return list;
    }

    #endregion

    #region Comment Ratings

    public async Task<Dictionary<string, float>> GetCommentRatingsAsync(int productId)
    {
        var comment = await GetCommentsByProductIdAsync(productId);
        float avgQuality = 0;
        float avgDesign = 0;
        float avgEase = 0;
        float avgValue = 0;
        float avgInnovation = 0;
        float avgFeatures = 0;
        foreach (var variable in comment)
        {
            avgQuality += variable.BuildQuality;
            avgDesign += variable.DesignAndAppearance;
            avgEase += variable.EaseOfUse;
            avgValue += variable.ValueForMoney;
            avgInnovation += variable.Innovation;
            avgFeatures += variable.Features;
        }

        avgQuality /= comment.Count;
        avgDesign /= comment.Count;
        avgEase /= comment.Count;
        avgValue /= comment.Count;
        avgInnovation /= comment.Count;
        avgFeatures /= comment.Count;
        var avgOverall = (avgQuality + avgDesign + avgEase + avgValue + avgInnovation + avgFeatures) / 6;
        avgOverall = (float)Math.Round(avgOverall, 1);
        var avg = new Dictionary<string, float>
        {
            { "avgQuality", avgQuality },
            { "avgDesign", avgDesign },
            { "avgEase", avgEase },
            { "avgValue", avgValue },
            { "avgInnovation", avgInnovation },
            { "avgFeatures", avgFeatures },
            { "avgOverall", avgOverall }
        };
        return avg;
    }

    public async Task<List<CommentViewModel>> GetCommentsByUserIdAsync(int userId)
    {
        var comments = await commentRepository.GetCommentsByUserIdAsync(userId);
        return comments.Select(x => new CommentViewModel
        {
            Id = x.Id,
            Title = x.Title,
            Content = x.Content,
            ProductId = x.ProductId,
            CreateDate = x.CreateDate
        }).ToList();
    }

    #endregion
}