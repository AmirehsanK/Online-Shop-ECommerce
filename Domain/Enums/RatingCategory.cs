using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum RatingCategory
    {
        BuildQuality,
        ValueForMoney,
        Innovation,
        Features,
        EaseOfUse,
        DesignAndAppearance
    }
    public static class RatingHelper
    {
        public static string GetRatingTitle(RatingCategory ratingType)
        {
            return ratingType switch
            {
                RatingCategory.BuildQuality => "کیفیت ساخت",
                RatingCategory.ValueForMoney => "ارزش خرید نسبت به قیمت",
                RatingCategory.Innovation => "نوآوری",
                RatingCategory.Features => "امکانات و قابلیت ها",
                RatingCategory.EaseOfUse => "سهولت استفاده",
                RatingCategory.DesignAndAppearance => "طراحی و ظاهر",
                _ => "Unknown Rating",
            };
        }
    }
}