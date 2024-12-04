using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public class TicketsEnum
    {
        public enum Section
        {
            Offer,
            CriticismorComplaint,
            OrderTracking,
            AfterSalesService,
            WarrantyInquiry

        }
        public enum Priority
        {
            Normal,
            Important,
            VeryImportant


        }
        public enum Status
        {
            IsAnswered,
            InProgress,
            IsClosed

        }
    }
}
