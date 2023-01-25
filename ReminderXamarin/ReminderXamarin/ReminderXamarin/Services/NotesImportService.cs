using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ReminderXamarin.Core.Interfaces.Services;
using Rm.Data.Data.Entities;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Services
{
    public class NotesImportService : INotesImportService
    {
        [Preserve]
        public NotesImportService()
        {
        }
        
        public List<Note> ImportNotes(string filePath)
        {
            string fileContext = File.ReadAllText(filePath);
            var allNotes = JsonConvert.DeserializeObject<List<Note>>(fileContext);
            return allNotes;
        }
    }
}