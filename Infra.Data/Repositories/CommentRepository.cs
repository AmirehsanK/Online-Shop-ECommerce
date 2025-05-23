﻿using Domain.Entities.Comment;
using Domain.Interface;
using Domain.ViewModel.Comment;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class CommentRepository(ApplicationDbContext context) : ICommentRepository
{
    #region Pending Comments

    public async Task<List<Comment>> GetAllPendingCommentsAsync()
    {
        return await context.Comments.Where(c => !c.IsApproved && !c.IsDeleted).ToListAsync();
    }

    #endregion

    #region Filter Comments

    public async Task<FilterCommentViewModel> GetAllCommentsAsync(FilterCommentViewModel filter)
    {
        var query = context.Comments.Where(p => !p.IsDeleted).AsQueryable();
        if (!string.IsNullOrEmpty(filter.Filter))
            query = filter.Filter switch
            {
                "approved" => query.Where(c => c.IsApproved),
                "notApproved" => query.Where(c => !c.IsApproved),
                _ => query
            };

        await filter.Paging(query.Select(p => new CommentViewModel
        {
            UserName = p.User.FirstOrDefault(u => u.Id == p.Id)!.FirstName + " " +
                       p.User.FirstOrDefault(u => u.Id == p.Id)!.LastName,
            Content = p.Content,
            Id = p.Id,
            IsApproved = p.IsApproved,
            ProductId = p.ProductId,
            Title = p.Title,
            Strengths = p.Strengths!,
            Weaknesses = p.Weaknesses!,
            CreateDate = p.CreateDate
        }));

        return filter;
    }

    #endregion

    #region Add Comment

    public async Task AddCommentAsync(Comment comment)
    {
        context.Comments.Add(comment);
        await context.SaveChangesAsync();
    }

    #endregion

    #region Delete Comment

    public async Task DeleteComment(Comment comment)
    {
        context.Comments.Update(comment);
        await context.SaveChangesAsync();
    }

    #endregion

    #region Comment Details

    public async Task<Comment> GetCommentByIdAsync(int id)
    {
        return (await context.Comments
            .Include(c => c.Interactions).Where(c => !c.IsDeleted)
            .FirstOrDefaultAsync(c => c.Id == id))!;
    }

    public async Task<List<Comment>> GetCommentByProductIdAsync(int productId)
    {
        return await context.Comments.Where(u => u.ProductId == productId && u.IsApproved == true).ToListAsync();
    }

    public async Task<List<Comment>> GetCommentsByUserIdAsync(int userId)
    {
        return await context.Comments.Where(c => c.UserId == userId && c.IsDeleted == false && c.IsApproved == true)
            .ToListAsync();
    }

    #endregion

    #region Comment Ratings

    public async Task<int> GetCommentLikesByIdAsync(int commentId)
    {
        var list = await context.UserInteraction.Where(u => u.CommentId == commentId).ToListAsync();
        return list.Count(item => item.IsLike);
    }

    public async Task<int> GetCommentDislikesByIdAsync(int commentId)
    {
        var list = await context.UserInteraction.Where(u => u.CommentId == commentId).ToListAsync();
        return list.Count(item => !item.IsLike);
    }

    #endregion

    #region Update Comment

    public async Task ApproveCommentAsync(int id)
    {
        var comment = await context.Comments.FindAsync(id);
        if (comment != null)
        {
            comment.IsApproved = true;
            await context.SaveChangesAsync();
        }
    }

    public async Task LikeCommentAsync(int commentId, string userIp, bool isLike)
    {
        var existingInteraction = await context.UserInteraction
            .FirstOrDefaultAsync(ui => ui.CommentId == commentId && ui.UserIp == userIp);

        if (existingInteraction == null)
        {
            var interaction = new UserInteraction
            {
                CommentId = commentId,
                UserIp = userIp,
                IsLike = isLike,
                Comment = null!
            };
            context.UserInteraction.Add(interaction);
        }
        else
        {
            existingInteraction.IsLike = isLike;
        }

        await context.SaveChangesAsync();
    }

    #endregion
}