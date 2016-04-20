using cs_challenge.Contracts;
using cs_challenge.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cs_challenge.Services
{
    public interface IUserService
    {
        UserContract CreateUser(CreateUserContract user);
        UserContract GetUser(Guid id);
        IEnumerable<User> GetUser(Func<User, bool> predicate);
    }
}