using EasyCaching.Core.Interceptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.Api.Models;

namespace Tools.Api.Services
{
    public interface IUserService
    {
        IList<User> getAll();

        [EasyCachingAble(Expiration = 10)]
        User getById(int id);

        User add(User user);

        User modify(User user);

        void delete(int id);
    }
}
