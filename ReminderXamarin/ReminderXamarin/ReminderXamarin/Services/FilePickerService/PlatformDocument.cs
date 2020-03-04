namespace ReminderXamarin.Services.FilePickerService
{
    public class PlatformDocument
    {
        public readonly string Name;
        public readonly string Path;

        public PlatformDocument(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}