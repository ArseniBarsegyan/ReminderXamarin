using ReminderXamarin.Core.Interfaces;
using ReminderXamarin.DependencyResolver;

namespace Rm.Helpers
{
    public static class ImageServiceHelper
    {
        private static readonly IImageService ImageService = ComponentFactory.Resolve<IImageService>();

        public static void ResizeImage(string sourceFile, string targetFile, int requiredWidth, int requiredHeight)
        {
            ImageService.ResizeImage(sourceFile, targetFile, requiredWidth, requiredHeight);
        }
    }
}