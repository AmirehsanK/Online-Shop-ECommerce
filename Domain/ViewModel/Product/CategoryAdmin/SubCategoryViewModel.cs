using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.ViewModel.Product.CategoryAdmin
{
    public class SubCategoryViewModel
    {
        public int Id { get; set; }
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }
        public int ParentId { get; set; }
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
    }
}
