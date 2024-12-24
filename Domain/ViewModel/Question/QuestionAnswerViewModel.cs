
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.Question
{
    public class QuestionAnswerViewModel
    {
        public int ProductId { get; set; }
        [Display(Name = "پرسش")]
        [MaxLength(300,ErrorMessage = "نمیتواند بیشتر از 300کارکتر باشد")]
        public string Question { get; set; }

        public int? Liked { get; set; }

        public int?  DissLiked { get; set; }


    }
}
