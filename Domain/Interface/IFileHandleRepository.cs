using Domain.Entities.Images;
using Domain.Enums;

namespace Domain.Interface;

public interface IFileHandleRepository
{
    Task SaveChangesAsync();

    #region Slider Images

    Task AddImageAsync(Banner banner);

    Task DeleteImage(Banner banner);

    Task<IEnumerable<Banner>> GetAllImagesAsync();

    Task<Banner> GetImageAsync(string guid);

    Task UpdateImage(Banner banner);

    #endregion

    #region Fixed Images

    Task AddFixedImageAsync(BannerFix banner);

    Task DeleteFixedImage(BannerFix banner);

    Task<IEnumerable<BannerFix>> GetAllFixedImagesAsync();

    Task<BannerFix> GetFixedImageAsync(string guid);

    Task UpdateFixedImage(BannerFix banner);
    
    Task<BannerFix> GetFixedImageByPositionAsync(ImageEnum.Banner position);

    #endregion
}