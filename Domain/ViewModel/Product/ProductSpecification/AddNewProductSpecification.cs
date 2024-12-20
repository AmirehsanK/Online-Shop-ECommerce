

using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.Product.ProductSpecification
{
    public class AddNewProductSpecification
    {
        public int  ProductId { get; set; }
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
        [Display(Name = "مقدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string? Values { get; set; }
    }
}
