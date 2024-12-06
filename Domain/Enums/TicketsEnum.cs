using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public class TicketsEnum
    {
        public enum Section
        { 
            [Display(Name = "پیشنهاد")]
            Offer,
            [Display(Name = "انتقاد یا شکایت")]
            CriticismorComplaint,
            [Display(Name = "پیگیری سفارش")]
            OrderTracking,
            [Display(Name = "خدمات پس از فروش")]
            AfterSalesService,
            [Display(Name = "استعلام گارانتی")]
            WarrantyInquiry,
            [Display(Name = "مدیریت")]
            Managment,
            [Display(Name = "حسایداری و امور مالی")]
            AccountingFinance,
            [Display(Name = "سایر موضوعات")]
            Other

        }
        public enum Priority
        {
            [Display(Name = "عادی")]
            Normal,
            [Display(Name = "مهم")]
            Important,
            [Display(Name = "خیلی مهم")]
            VeryImportant


        }
        public enum Status
        {
            [Display(Name = "پاسخ داده شده")]
            IsAnswered,
            [Display(Name = "در حال بررسی")]
            InProgress,
            [Display(Name = "بسته")]
            IsClosed

        }
    }
}
