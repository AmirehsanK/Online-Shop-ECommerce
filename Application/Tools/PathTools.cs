using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tools
{
    public class PathTools
    {
        #region Product

        public static string FileServerPath = Path.Join(Directory.GetCurrentDirectory(), "wwwroot/images/products/main/");
        public static string FilePath = "/images/products/main/";


        #endregion

        #region ProductGallery

        public static string ProductGalleryImageServerPath = Path.Join(Directory.GetCurrentDirectory(), "wwwroot/images/product-galleries/main/");
        public static string ProductGalleryImagePath = "/images/product-galleries/main/";
        public static string ProductGalleryThumbImageServerPath = Path.Join(Directory.GetCurrentDirectory(), "wwwroot/images/product-galleries/thumb/");
        public static string ProductGalleryThumbImagePath = "/images/product-galleries/thumb/";

        #endregion

    
    }
}
