﻿using Rm.Helpers;

using SQLite;

using SQLiteNetExtensions.Attributes;

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rm.Data.Data.Entities
{
    [Table(ConstantsHelper.Notes)]
    [Serializable]
    public class Note
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }

        [ForeignKey(typeof(UserModel))]
        public string UserId { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<GalleryItemModel> GalleryItems { get; set; }
    }
}