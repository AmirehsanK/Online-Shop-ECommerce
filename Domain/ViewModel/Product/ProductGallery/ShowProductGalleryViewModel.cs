
using Microsoft.AspNetCore.Http;

namespace Domain.ViewModel.Product.ProductGallery
{
    public class ShowProductGalleryViewModel
    {
        public int ProductId { get; set; }

        public List<IFormFile>? Gallery { get; set; }
    }
}
