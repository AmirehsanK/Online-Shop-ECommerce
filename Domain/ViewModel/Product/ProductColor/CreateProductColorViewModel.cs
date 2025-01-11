using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.Product.ProductColor;

public class CreateProductColorViewModel
{
    [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
    [Display(Name = "رنگ")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string ColorCode { get; set; }

    [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
    [Display(Name = "عنوان رنگ")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string Title { get; set; }
}