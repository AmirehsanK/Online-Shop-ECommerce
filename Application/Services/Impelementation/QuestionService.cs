using Application.Services.Interfaces;
using Domain.Entities.Question;
using Domain.Interface;
using Domain.ViewModel.Question;

namespace Application.Services.Impelementation;

public class QuestionService(IQuestionRepository questionRepository) : IQuestionService
{
    public async Task AddNewQuestionToProduct(QuestionAnswerViewModel model, int userid)
    {
        var Question = new QuestionAnswer
        {
            Question = model.Question,
            CreateDate = DateTime.Now,
            IsConfirmed = false,
            QuestionStatus = QuestionStatus.NotAnswered,
            IsClosed = false,
            ProductId = model.ProductId,
            UserId = userid
        };
        await questionRepository.AddAsync(Question);
        await questionRepository.SaveAsync();
    }

    #region Get Questions

    public async Task<FilterQuestionListViewModel> GetQuestion(FilterQuestionListViewModel filter)
    {
        return await questionRepository.GetQuestionAsync(filter);
    }

    public async Task<QuestionDetailViewModel> GetQuestionDetail(int questionId)
    {
        var question = await questionRepository.GetQuesetionById(questionId);
        var model = new QuestionDetailViewModel
        {
            Answer = question.Answer,
            ProductId = question.ProductId,
            Question = question.Question,
            UserId = question.UserId
        };
        return model;
    }

    public async Task<List<QuestionLikeViewModel>> GetProductQuestionsById(int productId)
    {
        var questionAnswers = await questionRepository.GetQuestionAnswers(productId);
        var liked = await questionRepository.GetLikedQuestions();
        var likeCount = liked.Count(u => u.ProductId == productId && u.QuestionLike == QuestionLike.Liked);
        var dissLikeCount = liked.Count(u => u.ProductId == productId && u.QuestionLike == QuestionLike.Disliked);
        return questionAnswers.Select(u => new QuestionLikeViewModel
        {
            DissLikeCount = dissLikeCount,
            LikeCount = likeCount,
            Answer = u.Answer,
            Question = u.Question
        }).ToList();
    }

    #endregion

    #region Manage Questions

    public async Task AddAnswerToQuestion(QuestionDetailViewModel model)
    {
        var Question = await questionRepository.GetQuesetionById(model.QuestionId);
        Question.Answer = model.Answer;
        Question.QuestionStatus = QuestionStatus.Answered;
        questionRepository.Update(Question);
        await questionRepository.SaveAsync();
    }

    public async Task CloseQuestion(int questionId)
    {
        var question = await questionRepository.GetQuesetionById(questionId);
        question.IsClosed = true;
        questionRepository.Update(question);
        await questionRepository.SaveAsync();
    }

    public async Task<bool> ToggleQuestionLike(int productId, int userId, QuestionLike questionLike)
    {
        var existingLike = await questionRepository.GeExitingLike(productId, userId);

        if (existingLike == null)
        {
            // اگر وجود ندارد، اضافه کن
            var newLike = new QuestionLiked
            {
                ProductId = productId,
                UserId = userId,
                QuestionLike = questionLike,
                CreateDate = DateTime.UtcNow
            };
            await questionRepository.AddQuestionLiked(newLike);
        }
        else
        {
            // اگر وجود دارد، به‌روزرسانی کن
            existingLike.QuestionLike = questionLike;
            existingLike.CreateDate = DateTime.UtcNow;
            questionRepository.UpdateLike(existingLike);
        }

        questionRepository.SaveAsync();
        return true;
    }

    public async Task ConfirmToShow(int questionId)
    {
        var question = await questionRepository.GetQuesetionById(questionId);
        question.IsConfirmed = true;
        questionRepository.Update(question);
        await questionRepository.SaveAsync();
    }

    #endregion
}