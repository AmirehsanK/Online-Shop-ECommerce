using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.User
{
    public class LoginUserViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        [DataType(DataType.EmailAddress, ErrorMessage = "ایمیل وارد شده معتبر نمیباشد")]
        public string Email { get; set; }
        [DataType(dataType: DataType.Password)]
        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Password { get; set; }

    }
}