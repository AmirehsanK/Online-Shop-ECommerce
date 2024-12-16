using Application.Extention;
using Application.Services.Interfaces;
using Application.Tools;
using Domain.Entities.Images;
using Domain.Enums;
using Domain.Interface;
using Domain.ViewModel.Image;

namespace Application.Services.Impelementation;

public class FileHandleService : IFileHandleService
{
    private readonly IFileHandleRepository _fileHandleRepository;

    public FileHandleService(IFileHandleRepository fileHandleRepository)
    {
        _fileHandleRepository = fileHandleRepository;
    }

    #region Slider Images

    public async Task<IEnumerable<Banner>> GetAllBanner()
    {
        return await _fileHandleRepository.GetAllImagesAsync();
    }

    public async Task<BannerViewModel> GetBanner(string guid)
    {
        var banner = await _fileHandleRepository.GetImageAsync(guid);
        //TODO Add dum image for not found image
        //if (banner == null)
        //    return DumImage

        return new BannerViewModel
        {
            Link = banner.Link,
            Title = banner.Title,
            ExpirationDate = banner.ExpirationDate
        };
    }

    public async Task<ImageEnum.Status> DeleteBanner(string guid)
    {
        var banner = await _fileHandleRepository.GetImageAsync(guid);
        if (banner == null)
            return ImageEnum.Status.Error;
        await _fileHandleRepository.DeleteImage(banner);
        banner.Title.DeleteImage(PathTools.BannerServerPath, null);
        await _fileHandleRepository.SaveChangesAsync();
        return ImageEnum.Status.Success;
    }

    public async Task<ImageEnum.Status> AddBanner(BannerViewModel banner)
    {
        var existingBanner = await _fileHandleRepository.GetImageAsync(banner.Title);
        if (existingBanner != null)
            return ImageEnum.Status.Error;

        var title = Guid.NewGuid().ToString("N") + Path.GetExtension(banner.Image.FileName);
        banner.Image.AddImageToServer(title, PathTools.BannerServerPath, null, null);
        await _fileHandleRepository.AddImageAsync(new Banner
        {
            Title = title,
            CreateDate = DateTime.Now,
            IsDeleted = false,
            Link = banner.Link,
            ExpirationDate = banner.ExpirationDate
        });
        await _fileHandleRepository.SaveChangesAsync();
        return ImageEnum.Status.Success;
    }

    public async Task<ImageEnum.Status> UpdateBanner(BannerViewModel banner)
    {
        var existingBanner = await _fileHandleRepository.GetImageAsync(banner.Title);
        if (existingBanner == null)
            return ImageEnum.Status.Error;

        var title = Guid.NewGuid().ToString("N") + Path.GetExtension(banner.Image.FileName);
        banner.Image.AddImageToServer(title, PathTools.BannerServerPath, null, null);
        banner.Title.DeleteImage(PathTools.BannerServerPath, null);
        existingBanner.Title = title;
        existingBanner.ExpirationDate= banner.ExpirationDate;
        existingBanner.Link = banner.Link;
        await _fileHandleRepository.UpdateImage(existingBanner);
        await _fileHandleRepository.SaveChangesAsync();
        return ImageEnum.Status.Success;
    }

    #endregion

    #region Fixed Images

    public async Task<IEnumerable<BannerFixViewModel>> GetAllFixedBanners()
    {
        var fixedBanners = await _fileHandleRepository.GetAllFixedImagesAsync();
        return fixedBanners.Select(f => new BannerFixViewModel
        {
            Id = f.Id,
            IsDeleted = f.IsDeleted,
            CreateDate = f.CreateDate,
            Title = f.Title,
            Link = f.Link
            ,Position=f.Position
        });
    }

    public async Task<BannerFixViewModel> GetBannerByPosition(ImageEnum.Banner position)
    {

        var banner = await _fileHandleRepository.GetFixedImageByPositionAsync(position);


        if (banner != null) 
        {
        return new BannerFixViewModel()
        {
            Id = banner.Id,
            Position = banner.Position,
            IsDeleted = banner.IsDeleted,
            CreateDate = banner.CreateDate,
            Title = banner.Title,
            Link = banner.Link
        }; 
        }
        return null;
    }
    public async Task<BannerFixViewModel> GetFixedBanner(string guid)
    {
        var banner = await _fileHandleRepository.GetFixedImageAsync(guid);
        if (banner == null)
            // TODO: Add dummy fixed banner logic if required
            return null;

        return new BannerFixViewModel
        {
            Id = banner.Id,
            IsDeleted = banner.IsDeleted,
            CreateDate = banner.CreateDate,
            Title = banner.Title,
            Link = banner.Link
        };
    }

    public async Task<ImageEnum.Status> AddFixedBanner(BannerFixViewModel banner)
    {
        var existingBanner = await _fileHandleRepository.GetFixedImageAsync(banner.Title);
        if (existingBanner != null)
            return ImageEnum.Status.Error;
        var existingPosition=await _fileHandleRepository.GetFixedImageByPositionAsync(banner.Position);
        if (existingPosition != null)
            return ImageEnum.Status.Error;
        var title = Guid.NewGuid().ToString("N") + Path.GetExtension(banner.Image.FileName);
        banner.Image.AddImageToServer(title, PathTools.FixedBannerServerPath, null, null);
        await _fileHandleRepository.AddFixedImageAsync(new BannerFix
        {
            Title = title,
            Link = banner.Link,
            Position = banner.Position,
            CreateDate = DateTime.Now,
            IsDeleted = false
        });
        await _fileHandleRepository.SaveChangesAsync();
        return ImageEnum.Status.Success;
    }

    public async Task<ImageEnum.Status> UpdateFixedBanner(BannerFixViewModel banner)
    {
        var existingBanner = await _fileHandleRepository.GetFixedImageAsync(banner.Title);
        if (existingBanner == null)
            return ImageEnum.Status.Error;
        
        var title = Guid.NewGuid().ToString("N") + Path.GetExtension(banner.Image.FileName);
        banner.Title.DeleteImage(PathTools.FixedBannerServerPath, null);
        banner.Image.AddImageToServer(title, PathTools.FixedBannerServerPath, null, null);
        existingBanner.Link = banner.Link;
        existingBanner.Title = title;
        await _fileHandleRepository.UpdateFixedImage(existingBanner);
        await _fileHandleRepository.SaveChangesAsync();
        return ImageEnum.Status.Success;
    }

    public async Task<ImageEnum.Status> DeleteFixedBanner(string guid)
    {
        var banner = await _fileHandleRepository.GetFixedImageAsync(guid);
        if (banner == null)
            return ImageEnum.Status.Error;
        banner.Title.DeleteImage(PathTools.FixedBannerPath, null);
        await _fileHandleRepository.DeleteFixedImage(banner);
        await _fileHandleRepository.SaveChangesAsync();
        return ImageEnum.Status.Success;
    }

    #endregion
}