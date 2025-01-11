namespace Domain.Enums;

public class ImageEnum
{
    public enum Banner
    {
        TopClosable,
        TopTwo,
        LeftSliderTop,
        LeftSliderBot,
        BotSliderRight,
        BotSliderLeft,
        BotRight,
        BotLeft
    }

    public enum Status
    {
        Success,
        Error
    }

    public static class BannerHelper
    {
        public static string GetBannerTitle(Banner bannerType)
        {
            return bannerType switch
            {
                Banner.TopClosable => "بالا (بسته شونده)",
                Banner.TopTwo => "دوم بالا",
                Banner.LeftSliderTop => "چپ اسلایدر بالا",
                Banner.LeftSliderBot => "چپ اسلایدر پایین",
                Banner.BotSliderRight => "پایین اسلایدر راست",
                Banner.BotSliderLeft => "پایین اسلایدر چپ",
                Banner.BotRight => "پایین راست",
                Banner.BotLeft => "پایین چپ",
                _ => "Unknown Banner"
            };
        }
    }
}