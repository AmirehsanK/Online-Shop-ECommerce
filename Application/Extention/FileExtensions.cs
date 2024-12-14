using Application.Security;
using Microsoft.AspNetCore.Http;

namespace Application.Extention;

public static class FileExtensions
{
    public static void AddImageToServer(this IFormFile image, string fileName, string orginalPath, int? width,
        int? height, string thumbPath = null, string deletefileName = null, bool checkImageContent = true)
    {
        if (image != null && image.IsImage(checkImageContent))
        {
            // Ensure the directory exists
            if (!Directory.Exists(orginalPath))
                Directory.CreateDirectory(orginalPath);

            // Delete existing files if necessary
            if (!string.IsNullOrEmpty(deletefileName))
            {
                if (File.Exists(Path.Combine(orginalPath, deletefileName)))
                    File.Delete(Path.Combine(orginalPath, deletefileName));

                if (!string.IsNullOrEmpty(thumbPath) && File.Exists(Path.Combine(thumbPath, deletefileName)))
                    File.Delete(Path.Combine(thumbPath, deletefileName));
            }

            // Save the new image
            var finalPath = Path.Combine(orginalPath, fileName);

            using (var stream = new FileStream(finalPath, FileMode.Create))
            {
                image.CopyTo(stream); // Copy the uploaded image to the server
            }

            // Handle thumbnail if required
            if (!string.IsNullOrEmpty(thumbPath))
            {
                if (!Directory.Exists(thumbPath))
                    Directory.CreateDirectory(thumbPath);

                var resizer = new ImageOptimizer();

                if (width != null && height != null)
                    resizer.ImageResizer(finalPath, Path.Combine(thumbPath, fileName), width.Value, height.Value);
            }
        }
    }

    public static void DeleteImage(this string imageName, string OriginPath, string? ThumbPath)
    {
        if (!string.IsNullOrEmpty(imageName))
        {
            if (File.Exists(OriginPath + imageName))
                File.Delete(OriginPath + imageName);

            if (!string.IsNullOrEmpty(ThumbPath))
                if (File.Exists(ThumbPath + imageName))
                    File.Delete(ThumbPath + imageName);
        }
    }

    public static async Task AddFilesToServer(this IFormFile file, string fileName, string orginalPath,
        string deletefileName = null, bool checkFileExtension = true)
    {
        if (file != null && file.IsFile(checkFileExtension))
        {
            if (!Directory.Exists(orginalPath))
                Directory.CreateDirectory(orginalPath);

            if (!string.IsNullOrEmpty(deletefileName))
                if (File.Exists(orginalPath + deletefileName))
                    File.Delete(orginalPath + deletefileName);

            if (!Directory.Exists(orginalPath))
                Directory.CreateDirectory(orginalPath);

            var finalPath = orginalPath + fileName;

            using (var stream = new FileStream(finalPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
    }

    public static void DeleteFile(this string fileName, string OriginPath)
    {
        if (!string.IsNullOrEmpty(fileName))
            if (File.Exists(OriginPath + fileName))
                File.Delete(OriginPath + fileName);
    }
}