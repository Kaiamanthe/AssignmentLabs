using SimpleNotes.Api.Interfaces;
using SimpleNotes.Api.Models;
using System.Xml.Linq;

namespace SimpleNotes.Api.Services
{
    public class NoteService : INoteService
    {
        private readonly List<Note> _notes = new();
        private int _nextId = 1;

        public IEnumerable<Note> GetAll()
        {
            return _notes;
        }

        public Note? GetById(int id)
        {
            return _notes.FirstOrDefault(n => n.Id == id);
        }

        public Note Add(Note note)
        {
            note.Id = _nextId++;
            _notes.Add(note);
            return note;
        }

        public void Delete(int id)
        {
            var note = GetById(id);
            if (note != null)
                _notes.Remove(note);
        }
    }
}
