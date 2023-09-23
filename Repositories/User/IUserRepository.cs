using NewsManagementMinimal.Models;

namespace NewsManagementMinimal.Repositories.User
{
    public interface IUserRepository
    {
        public Models.User? Get(UserLogin userLogin);
    }
}
