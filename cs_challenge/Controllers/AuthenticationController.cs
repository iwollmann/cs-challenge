using cs_challenge.Contracts;
using cs_challenge.Resources;
using cs_challenge.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Web.Http;

namespace cs_challenge.Controllers
{
    [RoutePrefix("api/authentication")]
    public class AuthenticationController : ApiController
    {
        private IAuthenticationService _authenticationService;
        private IUserService _userService;
        public AuthenticationController(IAuthenticationService authenticationService, IUserService userService)
        {
            if (authenticationService == null)
                throw new ArgumentNullException("AuthenticationService");

            if (userService == null)
                throw new ArgumentNullException("UserService");

            _authenticationService = authenticationService;
            _userService = userService;
        }

        [Route("signup")]
        public IHttpActionResult SignUp(CreateUserContract user)
        {
            UserContract newUser;
            try
            {
                newUser = _userService.CreateUser(user);
            }
            catch (ValidationException e) {
                throw new HttpResponseException(
                            Request.CreateErrorResponse(e.Code, e.Message));
            }
            catch (Exception)
            {
                throw;
            }

            return Created(string.Empty, newUser);
        }

        [Route("login")]
        public IHttpActionResult SignIn(LoginUserContract login) {
            UserContract user;
            try
            {
                user = _authenticationService.SignIn(login);
            }
            catch (ValidationException e)
            {
                throw new HttpResponseException(
                            Request.CreateErrorResponse(e.Code, e.Message));
            }
            catch (Exception)
            {
                throw;
            }

            return Content(HttpStatusCode.Accepted, user);
        }

        [Route("profile")]
        [AcceptVerbs("GET")]
        public IHttpActionResult Profile(string id = "") {
            UserContract user = null;
            try
            {
                Guid oid;
                if (string.IsNullOrEmpty(id) == false && Guid.TryParse(id, out oid))
                {
                    user = _userService.GetUser(oid);

                    if (user == null)
                        throw new HttpResponseException(
                            Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format(ApiResources.UserNotFound, id)));

                    if (_authenticationService.IsAuthorized(Request.Headers.Authorization, user) == false)
                    {
                        throw new HttpResponseException(
                            Request.CreateErrorResponse(HttpStatusCode.Unauthorized, string.Format(ApiResources.NotAuthorized)));
                    }
                }
                else
                    throw new Exception("Invalid querystring");
            }
            catch (ValidationException e)
            {
                throw new HttpResponseException(
                            Request.CreateErrorResponse(e.Code, e.Message));
            }
            catch (Exception)
            {

                throw;
            }

            return Content(HttpStatusCode.Accepted, user);
        }
    }
}
