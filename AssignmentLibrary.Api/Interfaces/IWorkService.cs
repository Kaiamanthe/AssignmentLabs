using AssignmentLibrary.Api.Models;

namespace AssignmentLibrary.Api.Interfaces
{
    public interface IWorkService
    {
        IEnumerable<Work> GetAll();
        Work? GetById(int id);
        Work Add(Work work);
        bool Update(Work work);
        void Delete(int id);
    }
}
