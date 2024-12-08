using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Security
{
    public static class CheckContentFile
    {
        public const int ImageMinimumBytes = 512;

        public static bool IsFile(this IFormFile postedFile, bool checkFileExtension = true)
        {
            if (checkFileExtension)
            {
                if (Path.GetExtension(postedFile.FileName)?.ToLower() != ".rar" &&
                    Path.GetExtension(postedFile.FileName)?.ToLower() != ".zip" &&
                    Path.GetExtension(postedFile.FileName)?.ToLower() != ".pdf"&&
                    Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"  &&
                    Path.GetExtension(postedFile.FileName).ToLower() != ".png"  &&
                     Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
                {
                    return false;
                }
            }

            return true;
        }

        public static bool HasLength(this IFormFile postedFile, int length)
        {
            if (postedFile.Length > length)
            {
                return false;
            }

            return true;
        }

        public static bool IsImage(this IFormFile postedFile, bool checkImage = true)
        {
            if (checkImage)
            {
                //-------------------------------------------
                //  Check the image mime types
                //-------------------------------------------
                if (postedFile.ContentType.ToLower() != "image/jpg" &&
                            postedFile.ContentType.ToLower() != "image/jpeg" &&
                            postedFile.ContentType.ToLower() != "image/x-png" &&
                            postedFile.ContentType.ToLower() != "image/png")
                {
                    return false;
                }

                //-------------------------------------------
                //  Check the image extension
                //-------------------------------------------
                if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                    && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                    && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
                {
                    return false;
                }

                //-------------------------------------------
                //  Attempt to read the file and check the first bytes
                //-------------------------------------------
                try
                {
                    if (!postedFile.OpenReadStream().CanRead)
                    {
                        return false;
                    }
                    //------------------------------------------
                    //check whether the image size exceeding the limit or not
                    //------------------------------------------ 
                    if (postedFile.Length < ImageMinimumBytes)
                    {
                        return false;
                    }

                    byte[] buffer = new byte[ImageMinimumBytes];
                    postedFile.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                    string content = Encoding.UTF8.GetString(buffer);
                    if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }

                //-------------------------------------------
                //  Try to instantiate new Bitmap, if .NET will throw exception
                //  we can assume that it's not a valid image
                //-------------------------------------------
                try
                {
                    using (var bitmap = new Bitmap(postedFile.OpenReadStream())) { }
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    postedFile.OpenReadStream().Position = 0;
                }

                return true;
            }

            return true;
        }

        public static bool IsVideo(this IFormFile postedFile)
        {
            //-------------------------------------------
            //  Check the video mime types
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".mp4"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".avi"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".wmv"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".mpeg-2"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".mov")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the video extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".mp4"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".avi"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".wmv"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".mpeg-2"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".mov")
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.OpenReadStream().CanRead)
                {
                    return false;
                }
                //------------------------------------------
                //check whether the image size exceeding the limit or not
                //------------------------------------------ 
                //if (postedFile.Length < ImageMinimumBytes)
                //{
                //    return false;
                //}

                byte[] buffer = new byte[ImageMinimumBytes];
                postedFile.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                string content = Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
