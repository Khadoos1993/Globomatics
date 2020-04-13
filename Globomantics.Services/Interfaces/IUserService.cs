using Globomantics.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Globomantics.Services.Interfaces
{
    public interface IUserService
    {
        User GetByUsernameAndPassword(string username, string password);

        User GetByGoogleEmail(string email);
    }
}
