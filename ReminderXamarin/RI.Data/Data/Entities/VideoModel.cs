using System;
using Realms;
using RI.Data.Data.Core;

namespace RI.Data.Data.Entities
{
    public class VideoModel : RealmObject, IEntity
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Path { get; set; }
        public Note Note { get; set; }
    }
}