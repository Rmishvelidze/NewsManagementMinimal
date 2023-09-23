using NewsManagementMinimal.Models;

namespace NewsManagementMinimal.Data
{
    public class UserDataContext
    {
        public static List<User?> Users = new()
        {
            new() { Username = "Nana_admin", EmailAddress = "Nana.admin@email.com", Password = "123", GivenName = "Nana", Surname = "Bibileishvili", Role = "Administrator" },
            new() { Username = "Megi_standard", EmailAddress = "Megi.standard@email.com", Password = "1234", GivenName = "Megi", Surname = "Shonia", Role = "Standard" },
            new() { Username = "Rezi_standard", EmailAddress = "Rezi.standard@email.com", Password = "12345", GivenName = "Rezi", Surname = "Mishvelidze", Role = "Standard" },
        };
    }
}
