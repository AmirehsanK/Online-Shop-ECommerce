using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.User;

public class ChangePasswordUserViewModel
{
    [MaxLength(50)]
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور فعلی")]
    [Required(ErrorMessage = "{0} الزامی است")]
    public string OldPassword { get; set; }

    [MaxLength(50)]
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور جدید")]
    [Required(ErrorMessage = "{0} الزامی است")]
    public string NewPassword { get; set; }

    [MaxLength(50)]
    [DataType(DataType.Password)]
    [Display(Name = "تکرار رمز عبور جدید")]
    [Required(ErrorMessage = "{0} الزامی است")]
    [Compare("Password", ErrorMessage = "تکرار رمز عبور اشتباه است")]
    public string ConfirmPassword { get; set; }
}