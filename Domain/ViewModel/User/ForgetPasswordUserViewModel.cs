using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.User;

public class ForgetPasswordUserViewModel

{
    [MaxLength(50)]
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور جدید")]
    [Required(ErrorMessage = "{0} الزامی است")]
    public string NewPassword { get; set; }

    [MaxLength(50)]
    [DataType(DataType.Password)]
    [Display(Name = "تکرار رمز عبور جدید")]
    [Required(ErrorMessage = "{0} الزامی است")]
    [Compare("NewPassword", ErrorMessage = "تکرار رمز عبور اشتباه است")]
    public string ConfirmPassword { get; set; }

    public string ActivationCode { get; set; }
}