using AssignmentLibrary.Api.Interfaces;
using AssignmentLibrary.Api.Models;

namespace AssignmentLibrary.Api.Services
{
    public class WorkService : IWorkService
    {
        private readonly List<Work> _works = new();
        private int _nextId = 1;
        public IEnumerable<Work> GetAll()
        {
            return _works;
        }
        public Work? GetById(int id)
        {
            return _works.FirstOrDefault(w => w.Id == id);
        }

        public Work Add(Work work)
        {
            work.Id = _nextId++;
            _works.Add(work);
            return work;
        }
        public bool Update(Work work)
        {
            var existingWork = GetById(work.Id);
            if (existingWork != null)
            {
                existingWork.Content = work.Content;
                return true;
            }
            return false;
        }
        public void Delete(int id)
        {
            var work = GetById(id);
            if (work != null)
            {
                _works.Remove(work);
            }
        }
    }
}
