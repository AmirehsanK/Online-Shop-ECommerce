
using Domain.Entities.Question;
using Domain.Interface;
using Infra.Data.Context;

namespace Infra.Data.Repositories
{
    public class QuestionRepository(ApplicationDbContext context):IQuestionRepository
    {
        public async Task AddAsync(QuestionAnswer questionAnswer)
        {
            await context.AddAsync(questionAnswer);
        }

        public void Update(QuestionAnswer questionAnswer)
        {
            context.Update(questionAnswer);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
