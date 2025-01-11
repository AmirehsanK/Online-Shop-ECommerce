using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities.Product;

public class ProductSpecification : BaseEntity
{
    [ForeignKey(nameof(Product))] public int ProductId { get; set; }

    public string Key { get; set; }

    public string Value { get; set; }

    #region Relation

    public Product Product { get; set; }

    #endregion
}