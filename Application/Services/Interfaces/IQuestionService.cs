using Domain.Entities.Question;
using Domain.ViewModel.Question;

namespace Application.Services.Interfaces;

public interface IQuestionService
{

    #region Question Management

    Task AddNewQuestionToProduct(QuestionAnswerViewModel model, int userid);
    Task<FilterQuestionListViewModel> GetQuestion(FilterQuestionListViewModel filter);
    Task<QuestionDetailViewModel> GetQuestionDetail(int questionId);
    Task<List<QuestionLikeViewModel>> GetProductQuestionsById(int productId);
    Task AddAnswerToQuestion(QuestionDetailViewModel model);
    Task CloseQuestion(int questionId);
    Task<bool> ToggleQuestionLike(int productId, int userId, QuestionLike questionLike);
    Task ConfirmToShow(int questionId);

    #endregion

}