using Core.Interfaces.Data.Repository;

namespace Task.Cheduler.Interfaces.Data.Tasks
{
    public interface ITaskRepository : IRepository<UserTask, int>
    {
    }
}
