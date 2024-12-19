using Domain.Entities.Account;
using Domain.Entities.Product;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Discount
{
    public class ProductDiscount
    {
        public int Id { get; set; } 
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        [ForeignKey(nameof(Discount))]
        public int DiscountId { get; set; }

        #region Relations
        public Discount Discount { get; set; }
        public Product.Product Product { get; set; }
        #endregion
    }
}
