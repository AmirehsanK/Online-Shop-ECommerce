using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.User;

public class LoginUserViewModel
{
    [MaxLength(50,ErrorMessage = "ایمیل وارد شده بیش از حد مجاز است")]
    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [DataType(DataType.EmailAddress, ErrorMessage = "ایمیل وارد شده معتبر نمیباشد")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [MaxLength(50, ErrorMessage = "رمز عبور وارد شده بیش از طول مجاز است")]
    [Display(Name = "رمز عبور")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}