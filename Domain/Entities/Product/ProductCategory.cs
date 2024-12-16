
using System.ComponentModel.DataAnnotations.Schema;

using Domain.Entities.Common;

namespace Domain.Entities.Product
{
    public class ProductCategory:BaseEntity
    {
        public int?  ParentId { get; set; }

        public string Title { get; set; }

        #region Relations 

        [ForeignKey(nameof(ParentId))]
        public ProductCategory Parent { get; set; }

        public ICollection<ProductCategory> Children { get; set; }

        public ICollection<Product> Products { get; set; }


        #endregion


    }
}
