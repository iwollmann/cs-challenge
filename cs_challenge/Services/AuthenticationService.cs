using AutoMapper;
using cs_challenge.Contracts;
using cs_challenge.Data;
using cs_challenge.Entities;
using cs_challenge.Resources;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Authentication;

namespace cs_challenge.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly EfDbContext _context;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private const string tokenPrefix = "bearer";

        public AuthenticationService(EfDbContext context, IMapper mapper, ITokenService tokenService)
        {
            if (mapper == null)
                throw new ArgumentNullException("Mapper");

            if (context == null)
                throw new ArgumentNullException("Context");

            if (tokenService == null)
                throw new ArgumentNullException("TokenService");

            _context = context;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public bool IsAuthorized(AuthenticationHeaderValue authorization, UserContract user)
        {
            if (authorization == null)
                throw new ValidationException(System.Net.HttpStatusCode.Unauthorized, Resources.ApiResources.NotAuthorized);

            var tokenExpired = (DateTime.UtcNow.TimeOfDay - user.LastLogin.Value.TimeOfDay).TotalMinutes > 30;
            if (tokenExpired)
                throw new ValidationException(System.Net.HttpStatusCode.Unauthorized, ApiResources.SessionExpired);

            var requestAuthorizationToken = authorization.ToString().Replace(tokenPrefix, "").Trim();

            return string.Equals(requestAuthorizationToken, user.AccessToken);
        }

        public UserContract SignIn(LoginUserContract login)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == login.Email);
            if (user != null) {
                if (user.Password == Security.SecurityHelper.GenerateHash(login.Password))
                {
                    user.LastLogin = DateTime.UtcNow;
                    _context.SaveChanges();

                    return _mapper.Map<User, UserContract>(user);
                }
                else
                    throw new ValidationException(System.Net.HttpStatusCode.Unauthorized, Resources.ApiResources.InvalidCredentials);
            }
            else
                throw new ValidationException(System.Net.HttpStatusCode.NoContent, Resources.ApiResources.InvalidCredentials);
        }

        public string RequestToken(User user)
        {
            return _tokenService.GenerateToken(user.Email, user.Password);
        }
    }
}