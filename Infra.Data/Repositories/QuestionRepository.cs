using Domain.Entities.Question;
using Domain.Interface;
using Domain.ViewModel.Question;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class QuestionRepository(ApplicationDbContext context) : IQuestionRepository
{

    #region Question Methods

    public async Task AddAsync(QuestionAnswer questionAnswer)
    {
        await context.AddAsync(questionAnswer);
    }

    public async Task<FilterQuestionListViewModel> GetQuestionAsync(FilterQuestionListViewModel filter)
    {
        var query = context.QuestionAnswers.AsQueryable();

        if (filter.ProductId.HasValue)
        {
            query = query.Where(u => u.ProductId == filter.ProductId);
        }

        switch (filter.ConfirmQuestion)
        {
            case ConfirmQuestion.All:
                break;
            case ConfirmQuestion.IsConfirmed:
                query = query.Where(u => u.IsConfirmed == true);
                break;
            case ConfirmQuestion.NotAccess:
                query = query.Where(u => u.IsConfirmed == false);
                break;
        }

        switch (filter.CloseQuestion)
        {
            case CloseQuestion.All:
                break;
            case CloseQuestion.Open:
                query = query.Where(u => u.IsClosed == false);
                break;
            case CloseQuestion.IsClosed:
                query = query.Where(u => u.IsClosed == true);
                break;
        }

        await filter.Paging(query.Select(u => new QuestionListViewModel()
        {
            ProductId = u.ProductId,
            DateTime = u.CreateDate,
            Question = u.Question,
            UserId = u.UserId,
            IsClosed = u.IsClosed,
            IsConfimed = u.IsConfirmed,
            Id = u.Id
        }));

        return filter;
    }

    public async Task<QuestionAnswer> GetQuesetionById(int questionId)
    {
        return (await context.QuestionAnswers.FindAsync(questionId))!;
    }

    public async Task<List<QuestionAnswer>> GetQuestionAnswers(int questionId)
    {
        return await context.QuestionAnswers
            .Where(u => u.IsClosed == false && u.ProductId == questionId && u.IsConfirmed == true)
            .ToListAsync();
    }

    public void Update(QuestionAnswer questionAnswer)
    {
        context.Update(questionAnswer);
    }

    #endregion

    #region Question Like Methods

    public async Task<QuestionLiked> GeExitingLike(int productId, int userId)
    {
        return (await context.QuestionLikes
            .FirstOrDefaultAsync(u => u.ProductId == productId && u.UserId == userId))!;
    }

    public void UpdateLike(QuestionLiked questionLiked)
    {
        context.Update(questionLiked);
    }

    public async Task<List<QuestionLiked>> GetLikedQuestions()
    {
        return await context.QuestionLikes.ToListAsync();
    }

    public async Task AddQuestionLiked(QuestionLiked questionLike)
    {
        await context.QuestionLikes.AddAsync(questionLike);
    }

    #endregion

    #region Save Changes

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }

    #endregion

}