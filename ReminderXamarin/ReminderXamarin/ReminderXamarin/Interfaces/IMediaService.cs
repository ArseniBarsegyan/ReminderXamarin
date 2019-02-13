namespace ReminderXamarin.Interfaces
{
    /// <summary>
    /// Provide ability to resize images.
    /// </summary>
    public interface IMediaService
    {
        /// <summary>
        /// Resize provided image.
        /// </summary>
        /// <param name="imageData">Content of the image to be resized</param>
        /// <param name="width">Image width</param>
        /// <param name="height">Image height</param>
        byte[] ResizeImage(byte[] imageData, float width, float height);

        /// <summary>
        /// Generate thumbnail picture of video from given time
        /// </summary>
        /// <param name="url">Video url.</param>
        /// <param name="second">Time of thumbnail.</param>
        /// <returns></returns>
        byte[] GenerateThumbImage(string url, long second);
    }
}