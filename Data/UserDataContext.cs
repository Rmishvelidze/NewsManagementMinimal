using NewsManagementMinimal.Models;

namespace NewsManagementMinimal.Data
{
    public class UserDataContext
    {
        public static List<User?> Users = new()
        {
            new() { Username = "Rezo_admin", EmailAddress = "Rezo.admin@email.com", Password = "123", GivenName = "Rezo", Surname = "Mishvelidze", Role = "Administrator" },
            new() { Username = "Maria_standard", EmailAddress = "Maria.standard@email.com", Password = "1234", GivenName = "Maria", Surname = "Smith", Role = "Standard" },
            new() { Username = "Cristina_standard", EmailAddress = "Cristina.standard@email.com", Password = "12345", GivenName = "Cristina", Surname = "Ivanovna", Role = "Standard" },
        };
    }
}
