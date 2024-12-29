
using Domain.Entities.Question;

namespace Domain.ViewModel.Question
{
    public class QuestionDetailViewModel
    {
        public int QuestionId { get; set; }
        public int ProductId { get; set; }

        public int UserId { get; set; }
        public string Question { get; set; }

        public bool IsClosed { get; set; }

        public bool IsConfirmed { get; set; }
        public string? Answer { get; set; }

        public QuestionStatus QuestionAnswers { get; set; }
    }
}
