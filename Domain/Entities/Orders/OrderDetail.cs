using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities.Orders;

public class OrderDetail : BaseEntity
{
    [ForeignKey(nameof(Order))] public int OrderId { get; set; }

    [ForeignKey(nameof(Product))] public int ProductId { get; set; }

    public int Count { get; set; }

    public int ColorPrice { get; set; }

    public int? ProductColorId { get; set; }

    #region Relation

    public Order Order { get; set; }

    public Product.Product Product { get; set; }

    #endregion
}