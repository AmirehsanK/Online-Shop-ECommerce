using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.User;

public class RegisterUserViewModel
{   [MaxLength(200)]
    [Display(Name = "نام")]
    public string? FirstName { get; set; }
    [MaxLength(200)]
    [Display(Name = "نام خانوادگی")]
    public string? LastName { get; set; }
    [MaxLength(50)]
    [EmailAddress]
    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "{0} الزامی است")]
    public string Email { get; set; }
    [MaxLength(50)]
    [DataType(dataType: DataType.Password)]
    [Display(Name = "رمز عبور")]
    [Required(ErrorMessage = "{0} الزامی است")]
    public string Password { get; set; }
    [MaxLength(50)]
    [DataType(dataType: DataType.Password)]
    [Display(Name = "تکرار رمز عبور")]
    [Required(ErrorMessage = "{0} الزامی است")]
    [Compare("Password", ErrorMessage = "تکرار رمز عبور اشتباه است")]
    public string ConfirmPassword { get; set; }
}