using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.ContactUs;

public class SendMessageViewModel
{
    [Required(ErrorMessage = "لطفاً پیام خود را وارد کنید.")]
    public string Message { get; set; }

    [Required(ErrorMessage = "لطفاً موضوع را وارد کنید.")]
    public string Subject { get; set; }

    [Required(ErrorMessage = "لطفاً ایمیل خود را وارد کنید.")]
    [EmailAddress(ErrorMessage = "لطفاً یک ایمیل معتبر وارد کنید.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "لطفاً شماره تلفن خود را وارد کنید.")]
    [Phone(ErrorMessage = "لطفاً یک شماره تلفن معتبر وارد کنید.")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "لطفاً نام کامل خود را وارد کنید.")]
    public string FullName { get; set; }
}