using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Rm.Data.Data.Entities;

namespace ReminderXamarin.Core.Interfaces
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
