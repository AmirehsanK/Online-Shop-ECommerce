using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.Product.CategoryAdmin
{
    public class BaseCategoryViewModel
    {
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

  



    }
}
