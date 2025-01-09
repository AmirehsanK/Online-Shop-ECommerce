using Domain.Entities.Faq;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class FaqRepository(ApplicationDbContext context) : IFaqRepository
{

    #region Faq Category Methods

    public async Task AddFaqCategory(FaqCategory faqCategory)
    {
        await context.FaqCategories.AddAsync(faqCategory);
    }

    public async Task<List<FaqCategory>> GetAllFaqCategory()
    {
        return await context.FaqCategories.Where(u => u.IsDeleted == false).ToListAsync();
    }

    public async Task<FaqCategory> GetFaqCategoryByIdAsync(int categoryid)
    {
        return (await context.FaqCategories.FindAsync(categoryid))!;
    }

    public void UpdateFaqCategory(FaqCategory faqCategory)
    {
        context.Update(faqCategory);
    }

    public async Task<FaqCategory> GetCategoryById(int id)
    {
        return (await context.FaqCategories.FindAsync(id))!;
    }

    #endregion

    #region Faq Question Methods

    public async Task AddQuestion(FaqQuestion question)
    {
        await context.FaqQuestions.AddAsync(question);
    }

    public async Task<List<FaqQuestion>> GetFaqQuestion(int categoryid)
    {
        return await context.FaqQuestions.Where(u => u.CategoryId == categoryid && u.IsDeleted == false).ToListAsync();
    }

    public async Task<FaqQuestion> GetQuestionById(int questionid)
    {
        return (await context.FaqQuestions.FindAsync(questionid))!;
    }

    public void UpdateFaqQuestion(FaqQuestion faqQuestion)
    {
        context.FaqQuestions.Update(faqQuestion);
    }

    public void UpdateListQuestion(List<FaqQuestion> list)
    {
        context.FaqQuestions.UpdateRange(list);
    }

    #endregion

    #region Save Changes

    public async Task SaveChangeAsync()
    {
        await context.SaveChangesAsync();
    }

    #endregion

}