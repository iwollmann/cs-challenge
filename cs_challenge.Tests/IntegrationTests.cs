using cs_challenge.Contracts;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace cs_challenge.Tests
{
    public class IntegrationTests
    {
        HttpClient client;
        public IntegrationTests()
        {
            var someApplication = new HttpApplication("http://localhost");

            client = new HttpClient(someApplication);
        }
        [Fact]
        public async Task ShouldBeAbleToSignUpNewUser()
        {
            var user1 = new { Nome = "add", Email = "test@mail.test.com", Senha = "qwerty" };
            var r1 = await client.PostAsJsonAsync("http://localhost/api/authentication/SignUp", user1);

            Assert.Equal(HttpStatusCode.Created, r1.StatusCode);

            var createdUser = await r1.Content.ReadAsAsync<UserContract>();
            Assert.NotEmpty(createdUser.AccessToken);
            Assert.Equal("add", createdUser.Name);
            Assert.Equal("test@mail.test.com", createdUser.Email);
        }

        [Fact]
        public async Task ShouldBeAbleToSignInUser()
        {
            var user1 = new { Nome = "login", Email = "login@mail.test.com", Senha = "qwerty" };
            var r1 = await client.PostAsJsonAsync("http://localhost/api/authentication/SignUp", user1);

            Assert.Equal(HttpStatusCode.Created, r1.StatusCode);

            var r2 = await client.PostAsJsonAsync("http://localhost/api/authentication/login", user1);

            Assert.Equal(HttpStatusCode.Accepted, r2.StatusCode);

            var loggedUser = await r2.Content.ReadAsAsync<UserContract>();

            Assert.NotEmpty(loggedUser.AccessToken);
            Assert.NotNull(loggedUser.LastLogin);
        }

        [Fact]
        public async Task ShouldBeAbleToViewProfileUser()
        {
            var user1 = new { Nome = "login", Email = "profile@mail.test.com", Senha = "qwerty" };
            var r1 = await client.PostAsJsonAsync("http://localhost/api/authentication/SignUp", user1);

            Assert.Equal(HttpStatusCode.Created, r1.StatusCode);

            var createdUser = await r1.Content.ReadAsAsync<UserContract>();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(string.Format("http://localhost/api/authentication/profile?id={0}", createdUser.Id)),
                Method = HttpMethod.Get
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", createdUser.AccessToken);

            var r2 = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Accepted, r2.StatusCode);

            var loggedUser = await r2.Content.ReadAsAsync<UserContract>();

            Assert.NotEmpty(loggedUser.AccessToken);
            Assert.NotNull(loggedUser.LastLogin);
        }
    }
}
