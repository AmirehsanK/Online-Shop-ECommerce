using Domain.Entities.Question;
using Domain.ViewModel.Question;

namespace Domain.Interface;

public interface IQuestionRepository
{
    #region Save Changes

    Task SaveAsync();

    #endregion

    #region Question Management

    Task AddAsync(QuestionAnswer questionAnswer);
    Task<FilterQuestionListViewModel> GetQuestionAsync(FilterQuestionListViewModel filter);
    Task<QuestionAnswer> GetQuesetionById(int questionId);
    Task<List<QuestionAnswer>> GetQuestionAnswers(int questionId);
    void Update(QuestionAnswer questionAnswer);

    #endregion

    #region Question Likes

    Task<QuestionLiked> GeExitingLike(int productId, int userId);
    Task<List<QuestionLiked>> GetLikedQuestions();
    Task AddQuestionLiked(QuestionLiked questionLike);
    void UpdateLike(QuestionLiked questionLiked);

    #endregion
}