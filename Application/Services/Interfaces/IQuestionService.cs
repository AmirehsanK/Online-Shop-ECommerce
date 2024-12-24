
using Domain.ViewModel.Question;

namespace Application.Services.Interfaces
{
    public interface IQuestionService
    {
        Task AddNewQuestionToProduct(QuestionAnswerViewModel model,int userid);

    }
}
