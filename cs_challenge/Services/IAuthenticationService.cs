using System;
using System.Net.Http.Headers;
using cs_challenge.Contracts;
using cs_challenge.Entities;

namespace cs_challenge.Services
{
    public interface IAuthenticationService
    {
        //UserContract CreateUser(CreateUserContract user);
        string RequestToken(User user);
        UserContract SignIn(LoginUserContract login);
        //UserContract GetUser(Guid id);
        bool IsAuthorized(AuthenticationHeaderValue authorization, UserContract user);
    }
}