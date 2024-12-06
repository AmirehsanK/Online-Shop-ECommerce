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

        public static string ProductImageServerPath = Path.Join(Directory.GetCurrentDirectory(), "wwwroot/images/products/main/");
        public static string ProductImagePath = "/images/products/main/";
        public static string ProductThumbImageServerPath = Path.Join(Directory.GetCurrentDirectory(), "wwwroot/images/products/thumb/");
        public static string ProductThumbImagePath = "/images/products/thumb/";
        public static string SliderImage = Path.Join(Directory.GetCurrentDirectory(), "wwwroot/images/products/Slider/");
        public static string SliderImageRoot = "/images/products/Slider/";

        #endregion

    
    }
}
