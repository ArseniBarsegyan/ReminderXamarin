using System;
using System.IO;

using ReminderXamarin.Core.Interfaces;

namespace ReminderXamarin.Droid.Services
{
    public class FileSystem : IFileSystem
    {
        public byte[] ReadAllBytes(string path)
        {
            try
            {
                return File.ReadAllBytes(path);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return new byte[0];
            }
        }
    }
}