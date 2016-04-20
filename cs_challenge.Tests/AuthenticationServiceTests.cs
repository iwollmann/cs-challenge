using AutoMapper;
using cs_challenge.Contracts;
using cs_challenge.Data;
using cs_challenge.Entities;
using cs_challenge.Mappers;
using cs_challenge.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace cs_challenge.Tests
{
    public class AuthenticationServiceTests
    {
        EfDbContext context;
        IMapper mapper;
        ITokenService mockTokenService;

        public AuthenticationServiceTests()
        {
            context = new EfDbContext(new InMemoryContext());
            mapper = AutoMapperConfig.RegisterMappings();
            mockTokenService = Substitute.For<ITokenService>();
        }

        [Fact]
        public void ShouldNotBeAbleToSignInWithNotRegisteredUser() {
            var sut = new AuthenticationService(context, mapper, mockTokenService);

            var user1 = new LoginUserContract() { Email = "empty@test.mail.com", Password = "qwerty" };

            var exception = Record.Exception(() => sut.SignIn(user1));
            Assert.NotNull(exception);
            Assert.IsType<ValidationException>(exception);
        }

        [Fact(Skip ="needs to resolve the context dependency, authorization service should not have this")]
        public void ShouldNotBeAbleToSignInWithIncorrectPassword() {
            var sut = new AuthenticationService(context, mapper, mockTokenService);

            var user1 = new LoginUserContract() { Email = "empty@test.mail.com", Password = "qwerty" };
            //context.Users.Add(new User() { Email = user1.Email, Password = Security.SecurityHelper.GenerateHash("fasfas") });

            var exception = Record.Exception(() => sut.SignIn(user1));
            Assert.NotNull(exception);
            Assert.IsType<ValidationException>(exception);
        }

        [Fact]
        public void ShouldNotBeAbleToViewProfileWithoutAuthorization() {
            var sut = new AuthenticationService(context, mapper, mockTokenService);

            var user1 = Substitute.For<UserContract>();
            
            var exception = Record.Exception(() => sut.IsAuthorized(null, user1));
            Assert.NotNull(exception);
            Assert.IsType<ValidationException>(exception);
        }

        [Fact]
        public void ShouldNotBeAbleToViewProfileWithInvalidAuthorization() {
            var sut = new AuthenticationService(context, mapper, mockTokenService);

            var user1 = new UserContract() {
                Id = Guid.NewGuid(),
                Name = "Empty",
                Email = "test@test.mail.com",
                LastLogin = DateTime.UtcNow,
                AccessToken = "fakeToken"
            };

            var authenticationHeader = new AuthenticationHeaderValue("bearer");

            Assert.False(sut.IsAuthorized(authenticationHeader, user1));
        }

        [Fact]
        public void ShouldNotBeAbleToViewProfileWithInvalidToken()
        {
            var sut = new AuthenticationService(context, mapper, mockTokenService);

            var user1 = new UserContract()
            {
                Id = Guid.NewGuid(),
                Name = "Empty",
                Email = "test@test.mail.com",
                LastLogin = DateTime.UtcNow.AddMinutes(-31),
                AccessToken = "fakeToken"
            };

            var authenticationHeader = new AuthenticationHeaderValue("bearer");

            var exception = Record.Exception(() => sut.IsAuthorized(authenticationHeader, user1));
            Assert.NotNull(exception);
            Assert.IsType<ValidationException>(exception);
        }
    }
}
