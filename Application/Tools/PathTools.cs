namespace Application.Tools;

public class PathTools
{
    #region Product

    public static string FileServerPath = Path.Join(Directory.GetCurrentDirectory(), "wwwroot/images/products/main/");
    public static string FilePath = "/images/products/main/";

    #endregion

    #region Banners

    public static string BannerServerPath =
        Path.Join(Directory.GetCurrentDirectory(), "wwwroot/images/Banners/Slider/");

    public static string BannerPath = "/images/Banners/Slider/";

    public static string FixedBannerServerPath =
        Path.Join(Directory.GetCurrentDirectory(), "wwwroot/images/Banners/Fixed/");

    public static string FixedBannerPath = "/images/Banners/Fixed/";

    #endregion

    #region Product Gallery

    public static string ProductGalleryImageServerPath =
        Path.Join(Directory.GetCurrentDirectory(), "wwwroot/images/product-galleries/main/");

    public static string ProductGalleryImagePath = "/images/product-galleries/main/";

    public static string ProductGalleryThumbImageServerPath =
        Path.Join(Directory.GetCurrentDirectory(), "wwwroot/images/product-galleries/thumb/");

    public static string ProductGalleryThumbImagePath = "/images/product-galleries/thumb/";

    #endregion

    #region Product Category

    public static string CategoryImageServerPath =
        Path.Join(Directory.GetCurrentDirectory(), "wwwroot/images/Product-Category/");

    public static string CategoryImagePath = "/images/Product-Category/";

    #endregion
}