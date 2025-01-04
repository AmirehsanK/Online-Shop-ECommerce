using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.Discount
{
    public class DiscountListAdminViewModel
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public bool IsPercentage { get; set; }
        public int Value { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
