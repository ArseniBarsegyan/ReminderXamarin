using System;
using Realms;
using RI.Data.Data.Core;

namespace RI.Data.Data.Entities
{
    public class PhotoModel : RealmObject, IEntity
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string ResizedPath { get; set; }
        public string Thumbnail { get; set; }
        public bool Landscape { get; set; }
        public bool IsVideo { get; set; }

        public Note Note { get; set; }
    }
}