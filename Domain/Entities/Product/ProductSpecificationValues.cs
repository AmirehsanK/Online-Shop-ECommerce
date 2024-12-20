using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
namespace Domain.Entities.Product
{
    public class ProductSpecificationValues:BaseEntity
    {
        [ForeignKey(nameof(ProductSpecification))]
        public int  ProductSpecificationId { get; set; }
        public string Value { get; set; }

        public ProductSpecification ProductSpecification { get; set; }
    }
}
