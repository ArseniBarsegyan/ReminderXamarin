using System;
using Realms;
using RI.Data.Data.Core;

namespace RI.Data.Data.Entities
{
    public class ToDoModel : RealmObject, IEntity
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public DateTimeOffset WhenHappens { get; set; }

        public AppUser AppUser { get; set; }
    }
}