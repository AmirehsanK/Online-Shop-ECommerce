using Domain.Entities.Images;
using Domain.Enums;
using Domain.ViewModel.Image;

namespace Application.Services.Interfaces;

public interface IFileHandleService
{
    #region Slider Images

    Task<IEnumerable<Banner>> GetAllBanner();

    Task<IEnumerable<Banner>> GetAllWorkingBanner();

    Task<BannerViewModel> GetBanner(string guid);

    Task<ImageEnum.Status> AddBanner(BannerViewModel banner);

    Task<ImageEnum.Status> DeleteBanner(string guid);

    Task<ImageEnum.Status> UpdateBanner(BannerViewModel banner);

    #endregion


    #region Fixed Images

    Task<BannerFixViewModel> GetBannerByPosition(ImageEnum.Banner position);
    Task<IEnumerable<BannerFixViewModel>> GetAllFixedBanners();

    Task<BannerFixViewModel> GetFixedBanner(string guid);

    Task<ImageEnum.Status> AddFixedBanner(BannerFixViewModel banner);

    Task<ImageEnum.Status> UpdateFixedBanner(BannerFixViewModel banner);

    Task<ImageEnum.Status> DeleteFixedBanner(string guid);

    #endregion
}