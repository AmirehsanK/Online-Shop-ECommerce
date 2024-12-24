
using Domain.Entities.Question;

namespace Domain.Interface
{
    public interface IQuestionRepository
    {
        Task AddAsync(QuestionAnswer questionAnswer);

        void Update(QuestionAnswer  questionAnswer);

        Task SaveAsync();
    }
}
