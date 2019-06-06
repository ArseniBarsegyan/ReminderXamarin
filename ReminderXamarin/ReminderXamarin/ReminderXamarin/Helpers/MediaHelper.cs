using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using PCLStorage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using ReminderXamarin.Helpers;
using Rm.Data.Data.Entities;

namespace Rm.Helpers
{
    /// <summary>
    /// Provide helper methods for taking photos and videos.
    /// </summary>
    public class MediaHelper
    {
        private readonly TransformHelper _transformHelper;

        public MediaHelper()
        {
            _transformHelper = new TransformHelper();
        }

        /// <summary>
        /// Launches the camera using Plugin.Media and lets the user take one photo.
        /// Store photo data in PhotoModel.
        /// </summary>
        public async Task<GalleryItemModel> TakePhotoAsync()
        {
            bool b = await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                return null;
            }

            var dt = DateTime.Now;
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Photos",
                Name = $"{dt:yyyyMMdd}_{dt:HHmmss}.jpg",
                SaveToAlbum = true
            });

            if (file != null)
            {
                var model = new GalleryItemModel();

                await _transformHelper.ResizeAsync(file.Path, model);
                await DeleteFileAsync(file.Path);

                return model;
            }
            return null;
        }

        /// <summary>
        /// Launches the camera using Plugin.Media and lets the user take video.
        /// Returns VideoModel that contains path to the video.
        /// </summary>
        /// <returns></returns>
        public async Task<GalleryItemModel> TakeVideoAsync()
        {
            bool b = await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
            {
                return null;
            }

            var dt = DateTime.Now;
            var file = await CrossMedia.Current.TakeVideoAsync(new StoreVideoOptions
            {
                Quality = VideoQuality.High,
                Directory = "Videos",
                Name = $"{dt:yyyyMMdd}_{dt:HHmmss}.mp4",
                SaveToAlbum = true
            });

            if (file != null)
            {
                var model = new GalleryItemModel
                {
                    VideoPath = file.Path
                };
                return model;
            }
            return null;
        }

        /// <summary>
        /// Deletes the PhotoModel and its pictures using PCLStorage.
        /// </summary>
        /// <param name="pm">The PhotoModel to be deleted</param>
        public async Task DeletePhotoModelAsync(GalleryItemModel galleryItemModel)
        {
            await DeleteFileAsync(galleryItemModel.ImagePath);
            await DeleteFileAsync(galleryItemModel.Thumbnail);
        }

        /// <summary>
        /// Deletes the file at the provided filepath using PCLStorage.
        /// </summary>
        /// <param name="filepath">The filepath to the file for deletion.</param>
        public async Task DeleteFileAsync(string filepath)
        {
            var file = await FileSystem.Current.GetFileFromPathAsync(filepath);
            try
            {
                await file.DeleteAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}