using System.Collections.Generic;
using System.Linq;
using ReminderXamarin.Data.Entities;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace ReminderXamarin.Data.Repositories
{
    public class NoteRepository
    {
        private readonly SQLiteConnection _db;
        private readonly List<Note> _notes;

        public NoteRepository(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<Note>();
            _db.CreateTable<PhotoModel>();
            _db.CreateTable<VideoModel>();
            _notes = new List<Note>(_db.GetAllWithChildren<Note>());
        }

        /// <summary>
        /// Get all notes from database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Note> GetAll()
        {
            return _notes;
        }

        /// <summary>
        /// Get note from database by id.
        /// </summary>
        /// <param name="id">Id of the note</param>
        /// <returns></returns>
        public Note GetNoteAsync(int id)
        {
            return _notes.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Create (if id = 0) or update note in database.
        /// </summary>
        /// <param name="note">Note to be saved</param>
        /// <returns></returns>
        public void Save(Note note)
        {
            if (note.Id != 0)
            {
                _notes.Insert(note.Id, note);
                _db.InsertOrReplaceWithChildren(note);
            }
            else
            {
                _notes.Add(note);
                _db.InsertWithChildren(note);
            }
        }

        /// <summary>
        /// Delete note from database.
        /// </summary>
        /// <param name="note">Note to be deleted</param>
        /// <returns></returns>
        public int DeleteNote(Note note)
        {
            _notes.Remove(note);
            return _db.Delete(note);
        }
    }
}