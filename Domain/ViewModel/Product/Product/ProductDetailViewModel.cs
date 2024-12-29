using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ViewModel.Product.ProductGallery;

namespace Domain.ViewModel.Product.Product
{
    public class ProductDetailViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public string ShortDescription { get; set; }

        public string? Review{ get; set; }
    
        public string? ExpertReview{ get; set; }

        public string ImageName { get; set; }

        public int Price { get; set; }

        public int OffPrice { get; set; } = 0;

        public int DiscountValue { get; set; } = 0;

        public int Inventory { get; set; } = 0;

        public ICollection<Entities.Product.ProductColor> ProductColors { get; set; }

        public ICollection<Entities.Product.ProductGallery> ProductGalleries { get; set; }
       
    }
}
