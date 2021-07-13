using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReminderXamarin.Core.Interfaces.Services;
using Rm.Data.Data.Entities;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Services
{
    public class UploadService : IUploadService
    {
        [Preserve]
        public UploadService()
        {
        }
        
        public async Task SendEmailWithAttachments(string subject, string body, IList<Note> notes)
        {
            var message = new EmailMessage
            {
                Subject = subject,
                Body = body,
                To = new List<string>{"Arseni.Barsegyan@gmail.com"},
                Attachments = new List<EmailAttachment> { CreateAttachment(notes) }
            };

            await Email.ComposeAsync(message);
        }

        private EmailAttachment CreateAttachment(IList<Note> notes)
        {
            var content = JsonConvert.SerializeObject(notes);
            var fileName = "Attachment.txt";
            var file = Path.Combine(FileSystem.CacheDirectory, fileName);
            File.WriteAllText(file, content);
            return new EmailAttachment(file);
        }
    }    
}
