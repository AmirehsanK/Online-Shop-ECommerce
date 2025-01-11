using Domain.Entities.Faq;
using Domain.ViewModel.Faq.Admin;
using Domain.ViewModel.Faq.Site;

namespace Application.Services.Interfaces;

public interface IFaqService
{

    #region Faq Category Management

    Task<List<GetAllFaqCategoryViewModel>> GetAllFaqCategoriesAsync();
    Task AddFaqCategory(AddFaqCategoryViewModel faqCategory);
    Task EditFaqCategory(EditFaqCategoryViewModel model);
    Task<EditFaqCategoryViewModel> GetCategoryForShow(int categoryid);
    Task<List<FaqCategory>> GetAllCategoryForShow();
    Task DeleteFaqCategoryAndChild(int categoryid);

    #endregion

    #region Faq Question Management

    Task AddFaqQuestion(CreateFaqQuestionViewModel model);
    Task<List<FaqQuestionListViewModel>> GetAllFaqQuestionListViewModelsAsync(int categegoryid);
    Task<EditFaqQuestionViewModel> GetFaqQuestionById(int id);
    Task EditFaqQuestion(EditFaqQuestionViewModel model);
    Task DeleteFaqQuestion(int id);

    #endregion

    #region Faq Site View

    Task<List<AllFaqCategoryViewModel>> GetAllCategoryForSite();
    Task<GetFaqCategoryAndChildViewModel> GetFaqCategories(int categoryid);

    #endregion

}