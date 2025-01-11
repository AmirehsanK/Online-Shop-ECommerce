using Application.Services.Interfaces;
using Domain.Entities.Faq;
using Domain.Interface;
using Domain.ViewModel.Faq.Admin;
using Domain.ViewModel.Faq.Site;

namespace Application.Services.Impelementation;

public class FaqService(IFaqRepository faqRepository) : IFaqService
{
    #region Faq Categories

    public async Task<List<GetAllFaqCategoryViewModel>> GetAllFaqCategoriesAsync()
    {
        var categories = await faqRepository.GetAllFaqCategory();
        return categories.Select(u => new GetAllFaqCategoryViewModel
        {
            CreateDate = u.CreateDate,
            Title = u.Title,
            Id = u.Id
        }).ToList();
    }

    public async Task AddFaqCategory(AddFaqCategoryViewModel faqCategory)
    {
        var category = new FaqCategory
        {
            CreateDate = DateTime.Now,
            Title = faqCategory.Title,
            IsDeleted = false
        };
        await faqRepository.AddFaqCategory(category);
        await faqRepository.SaveChangeAsync();
    }

    public async Task EditFaqCategory(EditFaqCategoryViewModel model)
    {
        var category = await faqRepository.GetFaqCategoryByIdAsync(model.Id);
        category.Title = model.Title;

        faqRepository.UpdateFaqCategory(category);
        await faqRepository.SaveChangeAsync();
    }

    public async Task<EditFaqCategoryViewModel> GetCategoryForShow(int categoryid)
    {
        var category = await faqRepository.GetFaqCategoryByIdAsync(categoryid);
        var model = new EditFaqCategoryViewModel
        {
            Title = category.Title,
            Id = category.Id
        };
        return model;
    }

    public async Task<List<FaqCategory>> GetAllCategoryForShow()
    {
        return await faqRepository.GetAllFaqCategory();
    }

    public async Task DeleteFaqCategoryAndChild(int categoryid)
    {
        var category = await faqRepository.GetCategoryById(categoryid);
        category.IsDeleted = true;
        var categoryQuestion = await faqRepository.GetFaqQuestion(categoryid);
        if (categoryQuestion != null!)
        {
            foreach (var item in categoryQuestion) item.IsDeleted = true;
            faqRepository.UpdateListQuestion(categoryQuestion);
        }

        faqRepository.UpdateFaqCategory(category);
        await faqRepository.SaveChangeAsync();
    }

    #endregion

    #region Faq Questions

    public async Task AddFaqQuestion(CreateFaqQuestionViewModel model)
    {
        var question = new FaqQuestion
        {
            CreateDate = DateTime.Now,
            IsDeleted = false,
            CategoryId = model.CategoryId,
            Description = model.Description,
            Question = model.Question
        };
        await faqRepository.AddQuestion(question);
        await faqRepository.SaveChangeAsync();
    }

    public async Task<List<FaqQuestionListViewModel>> GetAllFaqQuestionListViewModelsAsync(int categegoryid)
    {
        var questions = await faqRepository.GetFaqQuestion(categegoryid);
        return questions.Select(u => new FaqQuestionListViewModel
        {
            CreateDate = u.CreateDate,
            Answer = u.Description,
            Id = u.Id,
            Question = u.Question
        }).ToList();
    }

    public async Task<EditFaqQuestionViewModel> GetFaqQuestionById(int id)
    {
        var question = await faqRepository.GetQuestionById(id);
        var viewmodel = new EditFaqQuestionViewModel
        {
            Answer = question.Description,
            Question = question.Question,
            Id = question.Id,
            CategoryId = question.CategoryId
        };
        return viewmodel;
    }

    public async Task EditFaqQuestion(EditFaqQuestionViewModel model)
    {
        var question = await faqRepository.GetQuestionById(model.Id);
        question.CategoryId = model.CategoryId;
        question.Description = model.Answer;
        question.Question = model.Question;
        faqRepository.UpdateFaqQuestion(question);
        await faqRepository.SaveChangeAsync();
    }

    public async Task DeleteFaqQuestion(int id)
    {
        var question = await faqRepository.GetQuestionById(id);
        question.IsDeleted = true;
        faqRepository.UpdateFaqQuestion(question);
        await faqRepository.SaveChangeAsync();
    }

    #endregion

    #region Site Faq

    public async Task<List<AllFaqCategoryViewModel>> GetAllCategoryForSite()
    {
        var category = await faqRepository.GetAllFaqCategory();
        return category.Select(u => new AllFaqCategoryViewModel
        {
            CategoryTitle = u.Title,
            Categoryid = u.Id
        }).ToList();
    }

    public async Task<GetFaqCategoryAndChildViewModel> GetFaqCategories(int categoryid)
    {
        var category = await faqRepository.GetCategoryById(categoryid);
        var question = await faqRepository.GetFaqQuestion(categoryid);
        question.Select(u => new FaqQuestionCategoryViewModel
        {
            Question = u.Question,
            Answer = u.Description
        }).ToList();
        var faqQuestion = new GetFaqCategoryAndChildViewModel
        {
            Title = category.Title,
            Id = categoryid,
            ChildCategories = question
        };
        return faqQuestion;
    }

    #endregion
}