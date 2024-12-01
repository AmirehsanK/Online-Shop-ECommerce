using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel;

public class RegisterUserViewModel
{
    [Display(Name = "نام")]
    public string? FirstName { get; set; }
    [Display(Name = "نام خانوادگی")]
    public string? LastName { get; set; }
    [EmailAddress]
    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "{0} الزامی است")]
    public string Email { get; set; }
    [DataType(dataType: DataType.Password)]
    [Display(Name = "رمز عبور")]
    [Required(ErrorMessage = "{0} الزامی است")]
    public string Password { get; set; }
    [DataType(dataType: DataType.Password)]
    [Display(Name = "تکرار رمز عبور")]
    [Required(ErrorMessage = "{0} الزامی است")]
    [Compare("Password",ErrorMessage = "تکرار رمز عبور اشتباه است")]
    public string ConfirmPassword { get; set; }
}