using Globomantics.Core.Models;
using Globomantics.Services.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Globomantics.Services
{
    public class UserService : IUserService
    {
        private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Admin", LastName = "User", Username = "admin", Password = "admin", Role = "Admin", Email="kapil11102230@gmail.com" },
            new User { Id = 2, FirstName = "Normal", LastName = "User", Username = "user", Password = "user", Role = "User" }
        };

        public User GetByUsernameAndPassword(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);
            return user;

        }

        public User GetByGoogleEmail(string email)
        {
            var user = _users.SingleOrDefault(x => x.Email == email);
            return user;

        }
    }
}
