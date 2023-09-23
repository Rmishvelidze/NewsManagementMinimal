using NewsManagementMinimal.Data;
using NewsManagementMinimal.Models;

namespace NewsManagementMinimal.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        public Models.User? Get(UserLogin userLogin) =>
            UserDataContext.Users.FirstOrDefault
                (o => o != null && o.Username.Equals(userLogin.Username, StringComparison.OrdinalIgnoreCase)
                                                                && o.Password.Equals(userLogin.Password));
    }
}
