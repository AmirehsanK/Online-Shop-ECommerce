using System.ComponentModel.DataAnnotations;
using Domain.Attributes;

namespace Domain.ViewModel.User;

public class RegisterUserViewModel
{
    [MaxLength(200,ErrorMessage ="نام وارد شده بیش از حد مجاز است")]
    [MinLength(3,ErrorMessage ="حداقل کاراکتر وارد شده 3 میباشد")]
    [Display(Name = "نام")]
    public string? FirstName { get; set; }

    [MaxLength(200,ErrorMessage ="نام خانوادگی وارد شده بیش از حد مجاز است")]
    [MinLength(3,ErrorMessage ="حداقل کاراکتر وارد شده 3 میباشد")]
    [Display(Name = "نام خانوادگی")]
    public string? LastName { get; set; }

    [MaxLength(50,ErrorMessage = "ایمیل وارد شده بیش از حد مجاز است")]
    [EmailAddress(ErrorMessage = "ایمیل را به درستی وارد کنید")]
    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "{0} الزامی است")]
    public required string Email { get; set; }

    [Display(Name = "شماره تماس")]
    [Required(ErrorMessage = "{0} الزامی است")]
    [Phone(ErrorMessage = "شماره موبایل را به درستی وارد کنید")]
    [IranianPhoneNumber(ErrorMessage = "شماره موبایل را به درستی وارد کنید")]
    public required string PhoneNumber { get; set; }

    [MaxLength(50, ErrorMessage = "رمز عبور وارد شده بیش از طول مجاز است")]
    [MinLength(6,ErrorMessage = "حداقل 6 طول رمز عبور 6 کاراکتر است")]
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور")]
    [Required(ErrorMessage = "{0} الزامی است")]
    public required string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "تکرار رمز عبور")]
    [Required(ErrorMessage = "{0} الزامی است")]
    [Compare("Password", ErrorMessage = "تکرار رمز عبور اشتباه است")]
    public required string ConfirmPassword { get; set; }
}