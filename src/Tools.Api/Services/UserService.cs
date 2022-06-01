using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.Api.Models;

namespace Tools.Api.Services
{
    public class UserService : IUserService
    {
        public static IList<User> users;
        public UserService()
        {
            users = new List<User>
            {
                new User{ id = 1,username = "fanqi",password = "admin",enabled = 1},
                new User{ id = 2,username = "gaoxing",password = "admin",enabled = 1}
            };
        }
        public IList<User> getAll()
        {
            return users;
        }
        public User getById(int id)
        {
            return users.FirstOrDefault(f => f.id == id);
        }
        public User add(User user)
        {
            users.Add(user);
            return user;
        }
        public User modify(User user)
        {
            delete(user.id);
            add(user);
            return user;
        }
        public void delete(int id)
        {
            users.Remove(getById(id));
        }
    }
}
