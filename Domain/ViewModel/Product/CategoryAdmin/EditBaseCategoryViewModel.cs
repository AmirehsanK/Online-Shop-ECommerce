using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.Product.CategoryAdmin
{
    public class EditBaseCategoryViewModel
    {
        public int CategoryId { get; set; }

        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string CategoryTitle { get; set; }
    }
}
