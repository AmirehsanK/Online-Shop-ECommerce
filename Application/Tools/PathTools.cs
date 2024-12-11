namespace Application.Tools;

public class PathTools
{
    #region Product

    public static string FileServerPath = Path.Join(Directory.GetCurrentDirectory(), "wwwroot/images/products/main/");
    public static string FilePath = "/images/products/main/";

    #endregion

    #region Banners

    public static string BannerServerPath = Path.Join(Directory.GetCurrentDirectory(), "wwwroot/images/Banners/Slider/");
    public static string BannerPath = "/images/Banners/Slider/";

    public static string FixedBannerServerPath = Path.Join(Directory.GetCurrentDirectory(), "wwwroot/images/Banners/Fixed/");
    public static string FixedBannerPath = "/images/Banners/Fixed/";

    #endregion
}