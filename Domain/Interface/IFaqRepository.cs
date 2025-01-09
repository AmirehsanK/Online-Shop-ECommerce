using Domain.Entities.Faq;

namespace Domain.Interface;

public interface IFaqRepository
{

    #region Faq Category Management

    Task AddFaqCategory(FaqCategory faqCategory);
    Task<List<FaqCategory>> GetAllFaqCategory();
    Task<FaqCategory> GetFaqCategoryByIdAsync(int categoryid);
    void UpdateFaqCategory(FaqCategory faqCategory);
    Task<FaqCategory> GetCategoryById(int id);

    #endregion

    #region Faq Question Management

    Task AddQuestion(FaqQuestion question);
    Task<List<FaqQuestion>> GetFaqQuestion(int categoryid);
    Task<FaqQuestion> GetQuestionById(int questionid);
    void UpdateFaqQuestion(FaqQuestion faqQuestion);
    void UpdateListQuestion(List<FaqQuestion> list);

    #endregion

    #region Save Changes

    Task SaveChangeAsync();

    #endregion

}