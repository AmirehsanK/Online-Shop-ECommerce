using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Faq;
using Domain.ViewModel.Faq.Admin;
using Domain.ViewModel.Faq.Site;

namespace Application.Services.Interfaces
{
    public interface IFaqService
    {
        Task<List<GetAllFaqCategoryViewModel>> GetAllFaqCategoriesAsync();

        Task AddFaqCategory(AddFaqCategoryViewModel  faqCategory);
        Task EditFaqCategory(EditFaqCategoryViewModel model);

        Task<EditFaqCategoryViewModel> GetCategoryForShow(int categoryid);

        Task<List<FaqCategory>> GetAllCategoryForShow();

        Task AddFaqQuestion(CreateFaqQuestionViewModel model);

        Task<List<FaqQuestionListViewModel>> GetAllFaqQuestionListViewModelsAsync(int categegoryid);

        Task<EditFaqQuestionViewModel> GetFaqQuestionById(int id);

        Task EditFaqQuestion(EditFaqQuestionViewModel model);

        Task DeleteFaqCategoryAndChild(int categoryid);

        Task DeleteFaqQuestion(int id);

        Task<List<AllFaqCategoryViewModel>> GetAllCategoryForSite();

        Task<GetFaqCategoryAndChildViewModel> GetFaqCategories(int categoryid);
    }
}
