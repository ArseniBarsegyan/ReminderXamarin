using System.Collections.Generic;
using Rm.Data.Data.Entities;

namespace ReminderXamarin.Core.Interfaces.Services
{
    public interface INotesImportService
    {
        List<Note> ImportNotes(string filePath);
    }
}