using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.User;

public class ForgotPasswordMailUserViewModel
{
    [MaxLength(200)]
    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [DataType(DataType.EmailAddress, ErrorMessage = "ایمیل وارد شده معتبر نمیباشد")]
    [EmailAddress]
    public string Email { get; set; }
}