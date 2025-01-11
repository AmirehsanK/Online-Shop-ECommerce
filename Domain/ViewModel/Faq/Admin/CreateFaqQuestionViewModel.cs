using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.Faq.Admin;

public class CreateFaqQuestionViewModel
{
    public int CategoryId { get; set; }

    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
    [Display(Name = "سوال")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string Question { get; set; }

    [Display(Name = "جواب سوال")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string Description { get; set; }
}