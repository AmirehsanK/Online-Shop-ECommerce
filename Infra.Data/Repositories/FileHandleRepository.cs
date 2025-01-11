using Domain.Entities.Images;
using Domain.Enums;
using Domain.Interface;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class FileHandleRepository(ApplicationDbContext context) : IFileHandleRepository
{
    #region Save Changes

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    #endregion

    #region Slider Image

    public async Task AddImageAsync(Banner banner)
    {
        await context.Banners.AddAsync(banner);
    }

    public void DeleteImage(Banner banner)
    {
        context.Banners.Remove(banner);
    }

    public async Task<IEnumerable<Banner>> GetAllImagesAsync()
    {
        return await context.Banners.ToListAsync();
    }

    public async Task<IEnumerable<Banner>> GetAllWorkingImagesAsync()
    {
        var currentTime = DateTime.UtcNow;
        return await context.Banners
            .Where(b => b.ExpirationDate == null || b.ExpirationDate > currentTime)
            .ToListAsync();
    }

    public async Task<Banner> GetImageAsync(string guid)
    {
        return (await context.Banners.FirstOrDefaultAsync(x => x.Title == guid))!;
    }

    public void UpdateImage(Banner banner)
    {
        context.Banners.Update(banner);
    }

    #endregion

    #region Fixed Images

    public async Task AddFixedImageAsync(BannerFix banner)
    {
        await context.BannerFix.AddAsync(banner);
    }

    public void DeleteFixedImage(BannerFix banner)
    {
        context.BannerFix.Remove(banner);
    }

    public async Task<IEnumerable<BannerFix>> GetAllFixedImagesAsync()
    {
        return await context.BannerFix.ToListAsync();
    }

    public async Task<BannerFix> GetFixedImageAsync(string guid)
    {
        return (await context.BannerFix.FirstOrDefaultAsync(x => x.Title == guid))!;
    }

    public void UpdateFixedImage(BannerFix banner)
    {
        context.BannerFix.Update(banner);
    }

    public async Task<BannerFix> GetFixedImageByPositionAsync(ImageEnum.Banner position)
    {
        return (await context.BannerFix.FirstOrDefaultAsync(x => x.Position == position))!;
    }

    #endregion
}