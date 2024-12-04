using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.User
{
    public class LoginUserViewModel
    {
        [MaxLength(200)]
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        [DataType(DataType.EmailAddress, ErrorMessage = "ایمیل وارد شده معتبر نمیباشد")]
        public string Email { get; set; }
        [DataType(dataType: DataType.Password)]
        [MaxLength(200)]
        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Password { get; set; }
        
        public bool RememberMe { get; set; }
    }
}