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
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace cs_challenge.Tests
{
    public class UserServiceTests
    {
        EfDbContext context;
        IMapper mapper;
        IAuthenticationService mockAuthService;
        public UserServiceTests()
        {
            context = new EfDbContext(new InMemoryContext());
            mapper = AutoMapperConfig.RegisterMappings();
            mockAuthService = Substitute.For<IAuthenticationService>();
        }

        [Fact]
        public void ShouldNotBeAbleToCreateAnUserWithDuplicateEmail() {
            var sut = new UserService(context, mapper, mockAuthService);

            var user1 = new CreateUserContract() { Name = "User1", Email = "test@test.mail.com", Password="12345" };

            sut.CreateUser(user1);

            var user2 = new CreateUserContract() { Name="User2", Email = user1.Email, Password= "432535" };

            var exception = Record.Exception( () => sut.CreateUser(user2));
            Assert.NotNull(exception);
            Assert.IsType<ValidationException>(exception);
        }

        [Fact]
        public void ShouldBeAbleToGetUserById() {
            var sut = new UserService(context, mapper, mockAuthService);

            var user1 = new CreateUserContract() { Name = "User1", Email = "test2@test.mail.com", Password = "12345" };

            var createdUser = sut.CreateUser(user1);

            var user = sut.GetUser(createdUser.Id);

            Assert.NotNull(user);
            Assert.Equal(user1.Name, user.Name);
            Assert.Equal(user1.Email, user.Email);
            Assert.Equal(Security.SecurityHelper.GenerateHash(user1.Password), user.Password);
        }
    }
}
