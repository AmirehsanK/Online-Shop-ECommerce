using Application.Services.Interfaces;
using Domain.Entities.Faq;
using Domain.Interface;
using Domain.ViewModel.Faq.Admin;
using Domain.ViewModel.Faq.Site;

namespace Application.Services.Impelementation
{
    public class FaqService : IFaqService
    {
        #region Ctor

        private readonly IFaqRepository _faqRepository;

        public FaqService(IFaqRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }

        #endregion

        public async Task<List<GetAllFaqCategoryViewModel>> GetAllFaqCategoriesAsync()
        {
            var categories = await _faqRepository.GetAllFaqCategory();
            return categories.Select(u => new GetAllFaqCategoryViewModel()
            {
                CreateDate = u.CreateDate,
                Title = u.Title,
                Id = u.Id
            }).ToList();
        }

        public async Task AddFaqCategory(AddFaqCategoryViewModel faqCategory)
        {
            var Category = new FaqCategory()
            {
                CreateDate = DateTime.Now,
                Title = faqCategory.Title,
                IsDeleted = false
            };
            await _faqRepository.AddFaqCategory(Category);
            await _faqRepository.SaveChangeAsync();
        }

        public async Task EditFaqCategory(EditFaqCategoryViewModel model)
        {
            var category = await _faqRepository.GetFaqCategoryByIdAsync(model.Id);
            category.Title = model.Title;

            _faqRepository.UpdateFaqCategory(category);
            await _faqRepository.SaveChangeAsync();

        }

        public async Task<EditFaqCategoryViewModel> GetCategoryForShow(int categoryid)
        {
            var category = await _faqRepository.GetFaqCategoryByIdAsync(categoryid);
            var model = new EditFaqCategoryViewModel()
            {
                Title = category.Title,
                Id = category.Id
            };
            return model;
        }

        public async Task<List<FaqCategory>> GetAllCategoryForShow()
        {
            return await _faqRepository.GetAllFaqCategory();
        }

        public async Task AddFaqQuestion(CreateFaqQuestionViewModel model)
        {
            var question = new FaqQuestion()
            {
                CreateDate = DateTime.Now,
                IsDeleted = false,
                CategoryId = model.CategoryId,
                Description = model.Description,
                Question = model.Question
            };
            await _faqRepository.AddQuestion(question);
            await _faqRepository.SaveChangeAsync();

        }

        public async Task<List<FaqQuestionListViewModel>> GetAllFaqQuestionListViewModelsAsync(int categegoryid)
        {
            var questions = await _faqRepository.GetFaqQuestion(categegoryid);
            return questions.Select(u => new FaqQuestionListViewModel()
            {
                CreateDate = u.CreateDate,
                Answer = u.Description,
                Id = u.Id,
                Question = u.Question
            }).ToList();
        }

        public async Task<EditFaqQuestionViewModel> GetFaqQuestionById(int id)
        {
            var question = await _faqRepository.GetQuestionById(id);
            var viewmodel = new EditFaqQuestionViewModel()
            {
                Answer = question.Description,
                Question = question.Question,
                Id = question.Id,
                CategoryId = question.CategoryId,

            };
            return viewmodel;
        }

        public async Task EditFaqQuestion(EditFaqQuestionViewModel model)
        {
            var Question = await _faqRepository.GetQuestionById(model.Id);
            Question.CategoryId = model.CategoryId;
            Question.Description = model.Answer;
            Question.Question = model.Question;
            _faqRepository.UpdateFaqQuestion(Question);
            await _faqRepository.SaveChangeAsync();

        }

        public async Task DeleteFaqCategoryAndChild(int categoryid)
        {
            var Category = await _faqRepository.GetCategoryById(categoryid);
            Category.IsDeleted = true;
            var categoryQuestion = await _faqRepository.GetFaqQuestion(categoryid);
            if (categoryQuestion != null)
            {
                foreach (var item in categoryQuestion)
                {
                    item.IsDeleted = true;
                }
                _faqRepository.UpdateListQuestion(categoryQuestion);
            }

            _faqRepository.UpdateFaqCategory(Category);
            await _faqRepository.SaveChangeAsync();



        }

        public async Task DeleteFaqQuestion(int id)
        {
            var question = await _faqRepository.GetQuestionById(id);
            question.IsDeleted = true;
            _faqRepository.UpdateFaqQuestion(question);
            await _faqRepository.SaveChangeAsync();
        }

        public async Task<List<AllFaqCategoryViewModel>> GetAllCategoryForSite()
        {
            var category = await _faqRepository.GetAllFaqCategory();
            return category.Select(u => new AllFaqCategoryViewModel()
            {
                CategoryTitle = u.Title,
                Categoryid = u.Id
            }).ToList();
        }

        public async Task<GetFaqCategoryAndChildViewModel> GetFaqCategories(int categoryid)
        {
            var category = await _faqRepository.GetCategoryById(categoryid);
            var Question = await _faqRepository.GetFaqQuestion(categoryid);
            Question.Select(u => new FaqQuestionCategoryViewModel
            {
                Question = u.Question,
                Answer = u.Description
            }).ToList();
            var FaqQAuestion = new GetFaqCategoryAndChildViewModel()
            {
                Title = category.Title,
                Id = categoryid,
                ChildCategories = Question
            };
            return FaqQAuestion;




        }
    }
}
