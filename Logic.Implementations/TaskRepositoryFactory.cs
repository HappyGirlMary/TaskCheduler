using Task.Cheduler.Interfaces.Data.Tasks;
using Task.Cheduler.Logic.Implementations.Data.Tasks;

namespace Task.Cheduler.Logic.Implementations
{
    public static class TaskRepositoryFactory
    {
        public static ITaskRepository Create()
        {
            return new TaskJsonFileRepository();
        }        
    }
}
