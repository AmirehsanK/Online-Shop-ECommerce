using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Faq;

namespace Domain.Interface
{
    public interface IFaqRepository
    {
    
        Task AddFaqCategory(FaqCategory faqCategory);

        Task<List<FaqCategory>> GetAllFaqCategory();

        Task<FaqCategory> GetFaqCategoryByIdAsync(int categoryid);

        void UpdateFaqCategory(FaqCategory faqCategory);

        void UpdateFaqQuestion(FaqQuestion faqQuestion);

        Task SaveChangeAsync();

        Task AddQuestion(FaqQuestion  question);

        Task<FaqCategory> GetCategoryById(int id);

        Task<List<FaqQuestion>> GetFaqQuestion(int categoryid);

        Task<FaqQuestion> GetQuestionById(int questionid);

        void UpdateListQuestion(List<FaqQuestion> list);


    }
}
