using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.Faq.Admin;

public class AddFaqCategoryViewModel
{
    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
    [Display(Name = "سوال")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string Title { get; set; }
}