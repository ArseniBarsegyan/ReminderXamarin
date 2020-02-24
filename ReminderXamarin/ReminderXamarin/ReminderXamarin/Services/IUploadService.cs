using Rm.Data.Data.Entities;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReminderXamarin.Services
{
    public interface IUploadService
    {
        Task<HttpResult> UploadAll(IList<Note> notes, CancellationToken cancellationToken);
    }

    public enum HttpResult
    {
        Ok,
        Error
    }
}
