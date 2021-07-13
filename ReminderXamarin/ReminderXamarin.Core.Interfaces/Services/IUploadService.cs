using System.Collections.Generic;
using System.Threading.Tasks;
using Rm.Data.Data.Entities;

namespace ReminderXamarin.Core.Interfaces.Services
{
    public interface IUploadService
    {
        Task SendEmailWithAttachments(string subject, string body, IList<Note> notes);
    }
}
