

using Application.Services.Interfaces;
using Domain.Entities.Question;
using Domain.Interface;
using Domain.ViewModel.Question;

namespace Application.Services.Impelementation
{
    public class QuestionService(IQuestionRepository questionRepository):IQuestionService
    {
        public async Task AddNewQuestionToProduct(QuestionAnswerViewModel model,int userid)
        {
            var Question = new QuestionAnswer()
            {
                Question = model.Question,
                CreateDate = DateTime.Now,
                QuestionAnswers = QuestionStatus.NotConfirmed,
                ProductId = model.ProductId,
                UserId = userid
            };
            await questionRepository.AddAsync(Question);
            await questionRepository.SaveAsync();
        }
    }
}
