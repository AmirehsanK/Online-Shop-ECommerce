using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.ViewModel.Product.CategoryAdmin
{
    public class EditBaseCategoryViewModel
    {
        public int CategoryId { get; set; }

        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string CategoryTitle { get; set; }
        public int ParentId { get; set; }
        public string? ImageName { get; set; }
        public IFormFile? Image { get; set; }
    }
}
