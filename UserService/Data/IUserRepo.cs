using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Data
{
    public interface IUserRepo
    {
        bool SaveChanges();

        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetAllAdmins();
        IEnumerable<User> GetAllWheelsUsers();
        IEnumerable<User> GetAllNormalUsers();
        IEnumerable<User> GetAllNonActiveUsers();
        User? GetUserById(int id);
        void CreateUser(User user);
    }
}