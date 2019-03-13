namespace ReminderXamarin.Services
{
    /// <summary>
    /// Interface for retrieving files.
    /// </summary>
    public interface IFileHelper
    {
        /// <summary>
        /// Gets the file path with included filename.
        /// </summary>
        /// <returns>The file path.</returns>
        /// <param name="filename">Filename.</param>
        string GetLocalFilePath(string filename);

        /// <summary>
        /// Return directory for saving video.
        /// </summary>
        /// <param name="videoName">Video name.</param>
        /// <returns></returns>
        string GetVideoSavingPath(string videoName);
    }
}