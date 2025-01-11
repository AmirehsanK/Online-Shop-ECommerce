using Application.Extention;
using Application.Services.Interfaces;
using Application.Tools;
using Domain.Entities.Images;
using Domain.Enums;
using Domain.Interface;
using Domain.ViewModel.Image;

namespace Application.Services.Impelementation;

public class FileHandleService(IFileHandleRepository fileHandleRepository) : IFileHandleService
{
    
    #region Slider Images
    public async Task<IEnumerable<Banner>> GetAllBanner()
    {
        return await fileHandleRepository.GetAllImagesAsync();
    }

    public async Task<IEnumerable<Banner>> GetAllWorkingBanner()
    {
        return await fileHandleRepository.GetAllImagesAsync();
    }

    public async Task<BannerViewModel> GetBanner(string guid)
    {
        var banner = await fileHandleRepository.GetImageAsync(guid);

        return new BannerViewModel
        {
            Link = banner.Link,
            Title = banner.Title,
            ExpirationDate = banner.ExpirationDate?.ToShamsi().ToString()
        };
    }

    public async Task<ImageEnum.Status> DeleteBanner(string guid)
    {
        var banner = await fileHandleRepository.GetImageAsync(guid);
        if (banner == null!)
            return ImageEnum.Status.Error;
        fileHandleRepository.DeleteImage(banner);
        banner.Title.DeleteImage(PathTools.BannerServerPath, null);
        await fileHandleRepository.SaveChangesAsync();
        return ImageEnum.Status.Success;
    }

    public async Task<ImageEnum.Status> AddBanner(BannerViewModel banner)
    {
        var existingBanner = await fileHandleRepository.GetImageAsync(banner.Title);
        if (existingBanner != null!)
            return ImageEnum.Status.Error;

        var title = Guid.NewGuid().ToString("N") + Path.GetExtension(banner.Image!.FileName);
        banner.Image.AddImageToServer(title, PathTools.BannerServerPath, null, null);
        await fileHandleRepository.AddImageAsync(new Banner
        {
            Title = title,
            CreateDate = DateTime.Now,
            IsDeleted = false,
            Link = banner.Link,
            ExpirationDate = banner.ExpirationDate!.ToMiladiString(),
        });
        await fileHandleRepository.SaveChangesAsync();
        return ImageEnum.Status.Success;
    }

    public async Task<ImageEnum.Status> UpdateBanner(BannerViewModel banner)
    {
        var existingBanner = await fileHandleRepository.GetImageAsync(banner.Title);
        if (existingBanner == null!)
            return ImageEnum.Status.Error;
        if (banner.Image != null)
        {
            var title = Guid.NewGuid().ToString("N") + Path.GetExtension(banner.Image.FileName);
            banner.Image.AddImageToServer(title, PathTools.BannerServerPath, null, null);
            banner.Title.DeleteImage(PathTools.BannerServerPath, null);
            existingBanner.Title = title;
        }
        existingBanner.ExpirationDate = banner.ExpirationDate!.ToMiladiString();
        existingBanner.Link = banner.Link;
        fileHandleRepository.UpdateImage(existingBanner);
        await fileHandleRepository.SaveChangesAsync();
        return ImageEnum.Status.Success;
    }
    #endregion

    #region Fixed Images
    public async Task<IEnumerable<BannerFixViewModel>> GetAllFixedBanners()
    {
        var fixedBanners = await fileHandleRepository.GetAllFixedImagesAsync();
        return fixedBanners.Select(f => new BannerFixViewModel
        {
            Id = f.Id,
            IsDeleted = f.IsDeleted,
            CreateDate = f.CreateDate,
            Title = f.Title,
            Link = f.Link,
            Position = f.Position
        });
    }

    public async Task<BannerFixViewModel> GetBannerByPosition(ImageEnum.Banner position)
    {
        var banner = await fileHandleRepository.GetFixedImageByPositionAsync(position);

        if (banner != null!)
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
        return null!;
    }

    public async Task<BannerFixViewModel> GetFixedBanner(string guid)
    {
        var banner = await fileHandleRepository.GetFixedImageAsync(guid);
        if (banner == null!)
            return null!;

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
        var existingBanner = await fileHandleRepository.GetFixedImageAsync(banner.Title);
        if (existingBanner != null!)
            return ImageEnum.Status.Error;
        var existingPosition = await fileHandleRepository.GetFixedImageByPositionAsync(banner.Position);
        if (existingPosition != null!)
            return ImageEnum.Status.Error;
        var title = Guid.NewGuid().ToString("N") + Path.GetExtension(banner.Image!.FileName);
        banner.Image.AddImageToServer(title, PathTools.FixedBannerServerPath, null, null);
        await fileHandleRepository.AddFixedImageAsync(new BannerFix
        {
            Title = title,
            Link = banner.Link,
            Position = banner.Position,
            CreateDate = DateTime.Now,
            IsDeleted = false
        });
        await fileHandleRepository.SaveChangesAsync();
        return ImageEnum.Status.Success;
    }

    public async Task<ImageEnum.Status> UpdateFixedBanner(BannerFixViewModel banner)
    {
        var existingBanner = await fileHandleRepository.GetFixedImageAsync(banner.Title);
        if (existingBanner == null!)
            return ImageEnum.Status.Error;

        var title = Guid.NewGuid().ToString("N") + Path.GetExtension(banner.Image!.FileName);
        banner.Title.DeleteImage(PathTools.FixedBannerServerPath, null);
        banner.Image.AddImageToServer(title, PathTools.FixedBannerServerPath, null, null);
        existingBanner.Link = banner.Link;
        existingBanner.Title = title;
        fileHandleRepository.UpdateFixedImage(existingBanner);
        await fileHandleRepository.SaveChangesAsync();
        return ImageEnum.Status.Success;
    }

    public async Task<ImageEnum.Status> DeleteFixedBanner(string guid)
    {
        var banner = await fileHandleRepository.GetFixedImageAsync(guid);
        if (banner == null!)
            return ImageEnum.Status.Error;
        banner.Title.DeleteImage(PathTools.FixedBannerPath, null);
        fileHandleRepository.DeleteFixedImage(banner);
        await fileHandleRepository.SaveChangesAsync();
        return ImageEnum.Status.Success;
    }
    #endregion
    
}