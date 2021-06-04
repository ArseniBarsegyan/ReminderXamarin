using System;
using System.IO;
using ReminderXamarin.ViewModels.Base;
using Rm.Helpers;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class BirthdayViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public byte[] ImageContent { get; set; }

        public ImageSource PersonImageSource
        {
            get
            {
                if (ImageContent == null || ImageContent.Length == 0)
                {
                    return ImageSource.FromResource(
                        ConstantsHelper.NoPhotoImage);
                }

                return ImageSource.FromStream(
                    () => new MemoryStream(ImageContent));
            }
        }
        public string Name { get; set; }
        public DateTime BirthDayDate { get; set; }
        public string GiftDescription { get; set; }
        public string UserId { get; set; }
    }
}