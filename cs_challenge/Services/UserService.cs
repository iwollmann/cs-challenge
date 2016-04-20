using AutoMapper;
using cs_challenge.Contracts;
using cs_challenge.Data;
using cs_challenge.Entities;
using cs_challenge.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cs_challenge.Services
{
    public class UserService : IUserService
    {
        private readonly EfDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
        public UserService(EfDbContext context, IMapper mapper, IAuthenticationService authenticationService)
        {
            if (context == null)
                throw new ArgumentNullException("Context");

            if (mapper == null)
                throw new ArgumentNullException("Mapper");

            if (authenticationService == null)
                throw new ArgumentNullException("AuthenticationService");

            _context = context;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public UserContract CreateUser(CreateUserContract user)
        {
            if (_context.Users.Any(x => string.Equals(x.Email, user.Email)))
                throw new ValidationException(System.Net.HttpStatusCode.NoContent, ApiResources.DuplicateEmail);

            var newUser = _mapper.Map<CreateUserContract, User>(user);

            newUser.Password = Security.SecurityHelper.GenerateHash(user.Password);

            var token = _authenticationService.RequestToken(newUser);
            newUser.AccessToken = token;
            newUser.LastLogin = DateTime.UtcNow;

            var ee = _context.Add(newUser);
            _context.SaveChanges();

            return _mapper.Map<User, UserContract>(ee.Entity);
        }

        public UserContract GetUser(Guid id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            return _mapper.Map<User, UserContract>(user);
        }

        public IEnumerable<User> GetUser(Func<User, bool> predicate) {
            return _context.Users.Where(predicate);
        }
    }
}