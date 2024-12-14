using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Faq;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories
{
    public class FaqRepository:IFaqRepository
    {
        #region Ctor

        private readonly ApplicationDbContext _context;

        public FaqRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task AddFaqCategory(FaqCategory faqCategory)
        {
            await _context.FaqCategories.AddAsync(faqCategory);
        }

        public async Task<List<FaqCategory>> GetAllFaqCategory()
        {
            return await _context.FaqCategories.Where(u => u.IsDeleted == false).ToListAsync();
        }

        public async Task<FaqCategory> GetFaqCategoryByIdAsync(int categoryid)
        {
            return await _context.FaqCategories.FindAsync(categoryid);
        }

        public void UpdateFaqCategory(FaqCategory faqCategory)
        {
            _context.Update(faqCategory);
        }

        public void UpdateFaqQuestion(FaqQuestion faqQuestion)
        {
            _context.FaqQuestions.Update(faqQuestion);
        }


        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddQuestion(FaqQuestion question)
        {
            await _context.FaqQuestions.AddAsync(question);
        }

        public async Task<FaqCategory> GetCategoryById(int id)
        {
            return await _context.FaqCategories.FindAsync(id);
        }

        public async Task<List<FaqQuestion>> GetFaqQuestion(int categoryid)
        {
            return await _context.FaqQuestions.Where(u => u.CategoryId == categoryid&&u.IsDeleted==false).ToListAsync();
        }

        public async Task<FaqQuestion> GetQuestionById(int questionid)
        {
            return await _context.FaqQuestions.FindAsync(questionid);
        }

        public void UpdateListQuestion(List<FaqQuestion> list)
        {
            _context.FaqQuestions.UpdateRange(list);
        }
    }
}
