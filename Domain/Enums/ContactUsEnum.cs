using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public static class ContactUsEnum
{
    public enum Status
    {
        [Display(Name = "پاسخ داده شده")] IsAnswered,
        [Display(Name = "در حال بررسی")] InProgress
    }
}