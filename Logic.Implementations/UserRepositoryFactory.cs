using Task.Cheduler.Interfaces.Data.Users;
using Task.Cheduler.Logic.Implementations.Data.Users;

namespace Task.Cheduler.Logic.Implementations
{
    public static class UserRepositoryFactory
    {
        public static IUserRepository Create()
        {
            return new UserJsonFileRepository();
        }
    }
}
