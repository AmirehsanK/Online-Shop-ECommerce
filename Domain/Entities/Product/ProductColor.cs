

using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities.Product
{
    public class ProductColor : BaseEntity
    {
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        [ForeignKey(nameof(ColorId))]
        public int ColorId { get; set; }

        public int Count { get; set; }

        public int Price { get; set; }


        #region Relation

        public Color Color { get; set; }
        public Product Product { get; set; }

        #endregion



    }
}
