using OtelZone.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtelZone.Business.UserService
{
    public interface IUserService
    {
        Task<User> Authenticate(string email, string password);
        Task<bool> Register(string username, string usersurname, string useremail, string userphone, string password);
        Task<User> GetUserByEmail(string useremail);
    }
}
