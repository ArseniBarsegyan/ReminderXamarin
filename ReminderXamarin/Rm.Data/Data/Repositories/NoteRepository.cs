using Rm.Data.Data.Entities;

using SQLite;

using SQLiteNetExtensions.Extensions;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rm.Data.Data.Repositories
{
    public class NoteRepository
    {
        private readonly SQLiteConnection _db;

        public NoteRepository(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<Note>();
            _db.CreateTable<GalleryItemModel>();
        }

        public IEnumerable<Note> GetAll(Expression<Func<Note, bool>> filter = null)
        {
            return _db.GetAllWithChildren<Note>(filter);
        }

        public Note GetNoteAsync(int id)
        {
            return _db.GetWithChildren<Note>(id);
        }

        public void Save(Note note)
        {
            if (note.Id != 0)
            {
                _db.InsertOrReplaceWithChildren(note);
            }
            else
            {
                _db.InsertWithChildren(note);
            }
        }

        public int DeleteNote(Note note)
        {
            return _db.Delete(note);
        }
    }
}