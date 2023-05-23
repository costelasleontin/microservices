using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Data
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;

        public UserRepo(AppDbContext context)
        {
            _context = context;
        }
        public void CreateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Users.Add(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public IEnumerable<User> GetAllAdmins()
        {
            return _context.Users.Where(u => u.IsAdmin == true && u.IsActive == true).ToList();
        }


        public IEnumerable<User> GetAllNormalUsers()
        {
            return _context.Users.Where(u => u.IsAdmin == false && u.IsWheel == false && u.IsActive == true).ToList();
        }

        public IEnumerable<User> GetAllWheelsUsers()
        {
            return _context.Users.Where(u => u.IsWheel == true && u.IsActive == true).ToList();
        }

        public IEnumerable<User> GetAllNonActiveUsers()
        {
            return _context.Users.Where(u => u.IsActive == false).ToList();
        }

        public User? GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

    }
}