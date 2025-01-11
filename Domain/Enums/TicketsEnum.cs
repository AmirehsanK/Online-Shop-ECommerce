using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public class TicketsEnum
{
    public enum Priority
    {
        [Display(Name = "عادی")] Normal,
        [Display(Name = "مهم")] Important,
        [Display(Name = "خیلی مهم")] VeryImportant
    }

    public enum Section
    {
        [Display(Name = "پیشنهاد")] Offer,
        [Display(Name = "انتقاد یا شکایت")] CriticismorComplaint,
        [Display(Name = "پیگیری سفارش")] OrderTracking,
        [Display(Name = "خدمات پس از فروش")] AfterSalesService,
        [Display(Name = "استعلام گارانتی")] WarrantyInquiry,
        [Display(Name = "مدیریت")] Managment,

        [Display(Name = "حسایداری و امور مالی")]
        AccountingFinance,
        [Display(Name = "سایر موضوعات")] Other
    }

    public enum Status
    {
        [Display(Name = "پاسخ داده شده")] IsAnswered,
        [Display(Name = "در حال بررسی")] InProgress,
        [Display(Name = "بسته")] IsClosed
    }
}