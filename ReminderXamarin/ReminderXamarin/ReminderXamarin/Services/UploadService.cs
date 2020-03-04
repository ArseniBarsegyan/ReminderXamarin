using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

using ReminderXamarin.Core.Interfaces;

using Rm.Data.Data.Entities;
using Rm.Helpers;

namespace ReminderXamarin.Services
{
    public class UploadService : IUploadService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<HttpResult> UploadAll(IList<Note> notes, CancellationToken cancellationToken)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, notes);

                var content = new StreamContent(stream);

                var result = await _httpClient
                    .PostAsync(ConstantsHelper.NotesUploadUrl, content, cancellationToken)
                    .ConfigureAwait(false);
                if (result.IsSuccessStatusCode)
                {
                    return HttpResult.Ok;
                }
                return HttpResult.Error;
            }
        }
    }    
}
