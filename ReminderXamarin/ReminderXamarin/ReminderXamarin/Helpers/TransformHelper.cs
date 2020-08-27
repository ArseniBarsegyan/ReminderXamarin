using System.IO;
using System.Threading.Tasks;

using ExifLib;

using PCLStorage;

using ReminderXamarin.Core.Interfaces;

using Rm.Data.Data.Entities;

namespace ReminderXamarin.Helpers
{
    public class TransformHelper
    {
        private bool _landscape;
        private readonly IImageService _imageService;

        public TransformHelper(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task ResizeAsync(string filePath, GalleryItemModel galleryItemModel)
        {
            _landscape = false;
            var str = await ResizeAsync(filePath);
            galleryItemModel.ImagePath = str[0];
            galleryItemModel.Thumbnail = str[1];
            galleryItemModel.Landscape = _landscape;
        }

        private async Task<string[]> ResizeAsync(string filePath)
        {
            var str = new string[2];

            string folder = Path.GetDirectoryName(filePath) + "/";
            string img = folder + "R" + Path.GetFileName(filePath);
            string thumb = folder + "T" + Path.GetFileName(filePath);

            IFile vf = await FileSystem.Current.GetFileFromPathAsync(filePath);

            using (Stream stream = await vf.OpenAsync(PCLStorage.FileAccess.Read))
            {
                JpegInfo exif = ExifReader.ReadJpeg(stream);

                int width = 0;
                int height = 0;
                int thumbWidth = 0;
                int thumbHigh = 0;

                if (exif.Width > 0)
                {
                    width = exif.Width;
                    height = exif.Height;
                    if (width > height)
                    {
                        var temp = width;
                        width = height;
                        height = temp;

                        _landscape = true;
                    }
                }
                else
                {
                    width = 1000;
                    height = 2000;
                }
                if (exif.Width / 7 < 100)
                {
                    thumbWidth = 70;
                    thumbHigh = 100;
                }
                else
                {
                    thumbWidth = width / 7;
                    thumbHigh = height / 13;
                }

                _imageService.ResizeImage(filePath, img, width, height);
                _imageService.ResizeImage(filePath, thumb, thumbWidth, thumbHigh);

                str[0] = img;
                str[1] = thumb;
            }
            
            return str;
        }
    }
}