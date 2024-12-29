
using Domain.Entities.Question;
using Domain.ViewModel.Question;

namespace Domain.Interface
{
    public interface IQuestionRepository
    {
        Task AddAsync(QuestionAnswer questionAnswer);


        Task<FilterQuestionListViewModel> GetQuestionAsync(FilterQuestionListViewModel filter);

        Task<QuestionAnswer> GetQuesetionById(int questionId);

        Task<QuestionLiked> GeExitingLike(int productId, int userId);

        Task<List<QuestionAnswer>> GetQuestionAnswers(int questionId);
        void Update(QuestionAnswer  questionAnswer);
        void UpdateLike(QuestionLiked questionLiked);

        Task<List<QuestionLiked>> GetLikedQuestions();

        Task AddQuestionLiked(QuestionLiked  questionLike);

        Task SaveAsync();
    }
}
