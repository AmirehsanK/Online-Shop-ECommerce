
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.Product.ProductColor
{
    public class ColorListViewModel
    {
        public int Id { get; set; }
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
        [Display(Name = "عنوان رنگ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد")]
        [Display(Name = "کد رنگ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ColorCode { get; set; }

        public DateTime CreateDate { get; set; }


    }
}
