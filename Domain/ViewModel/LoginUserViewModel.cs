using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel
{
    public class LoginUserViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Email { get; set; }
        [DataType(dataType: DataType.Password)]
        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Password { get; set; }
    
    }
}