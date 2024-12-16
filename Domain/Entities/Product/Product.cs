
using System.ComponentModel.DataAnnotations;
using Domain.Entities.Common;

namespace Domain.Entities.Product
{
    public class Product:BaseEntity
    {
        [MaxLength(200)]
        public string ProductName { get; set; }

        [MaxLength(200)]
        public string ShortDescription { get; set; }

        public string Review{ get; set; }
    
        public string ExpertReview{ get; set; }

        [MaxLength(200)]
        public string ImageName { get; set; }

        [MaxLength(200)]
        public int Price { get; set; }
    
        public int Inventory { get; set; }

        public int CategoryId {  get; set; }

        #region Relation

        public ICollection<ProductGallery> ProductGalleries { get; set; }

        #endregion
    }
}
