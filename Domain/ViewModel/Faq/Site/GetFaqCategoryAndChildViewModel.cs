

using Domain.Entities.Faq;

namespace Domain.ViewModel.Faq.Site
{
    public class GetFaqCategoryAndChildViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<FaqQuestion> ChildCategories { get; set; }
    }
}
