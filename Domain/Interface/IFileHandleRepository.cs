using Domain.Entities.Images;
using Domain.Enums;

namespace Domain.Interface;

public interface IFileHandleRepository
{
    Task SaveChangesAsync();

    #region Slider Images

    Task AddImageAsync(Banner banner);

    void DeleteImage(Banner banner);

    Task<IEnumerable<Banner>> GetAllImagesAsync();

    Task<IEnumerable<Banner>> GetAllWorkingImagesAsync();

    Task<Banner> GetImageAsync(string guid);

    void UpdateImage(Banner banner);

    #endregion

    #region Fixed Images

    Task AddFixedImageAsync(BannerFix banner);

    void DeleteFixedImage(BannerFix banner);

    Task<IEnumerable<BannerFix>> GetAllFixedImagesAsync();

    Task<BannerFix> GetFixedImageAsync(string guid);

    void UpdateFixedImage(BannerFix banner);
    
    Task<BannerFix> GetFixedImageByPositionAsync(ImageEnum.Banner position);

    #endregion
}