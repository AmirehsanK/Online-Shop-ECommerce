using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities.Product;

public class ProductCategory : BaseEntity
{
    public int? ParentId { get; set; }

    [MaxLength(200)] public string Title { get; set; }

    public string? ImageName { get; set; }

    #region Relations

    [ForeignKey(nameof(ParentId))] public ProductCategory Parent { get; set; }

    public ICollection<ProductCategory> Children { get; set; }

    public ICollection<Product> Products { get; set; }

    #endregion
}