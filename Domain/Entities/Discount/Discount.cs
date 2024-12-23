using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Discount
{
    public class Discount:BaseEntity
    {
        public string? Code { get; set; }
        public bool IsPercentage { get; set; } 
        public int Value { get; set; } 
        public DateTime? StartDate { get; set; } 
        public DateTime? EndDate { get; set; } 
        public bool IsActive { get; set; } 
        public int? UsageLimit { get; set; }

        #region Relations
        public ICollection<UserDiscount> UserDiscounts { get; set; }
        public ICollection<ProductDiscount> ProductDiscounts { get; set; }
        #endregion
    }
}
