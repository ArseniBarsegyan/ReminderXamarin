using System;
using System.Collections.Generic;
using Realms;
using RI.Data.Data.Core;

namespace RI.Data.Data.Entities
{
    public class Note : RealmObject, IEntity
    {
        public Note()
        {
            Photos = new List<PhotoModel>();
            Videos = new List<VideoModel>();
        }

        [PrimaryKey]
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset EditDate { get; set; }

        public string UserId { get; set; }
        public AppUser AppUser { get; set; }

        public IList<PhotoModel> Photos { get; }
        public IList<VideoModel> Videos { get; }
    }
}