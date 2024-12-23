using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Common;

namespace Domain.Entities.Product
{
    public class ProductSpecification:BaseEntity
    {
        [ForeignKey(nameof(Product))]
        public int  ProductId { get; set; }
        public string Key { get; set; }

        public string Value { get; set; }

        #region Relation

        public Product Product { get; set; }

        #endregion
    

    }
}
