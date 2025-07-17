using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnitTestExamples.Data;
using UnitTestExamples.Models;
using UnitTestExamples.Services;


namespace UnitTestExamples.Tests.Services
{
    public class UserServiceTests
    {

        private UserContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) //Her test için ramde yeni bir veritabanı oluşturup testlerin eski bilgilerden etkilenmesini enhellemek için
                .Options;

            return new UserContext(options);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldAddUserToDatabase() 
        {
            // arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var userService = new UserService(context);

            var user = new User { Id = 1, Name = "Test" };

            // act fonksiyonun çağrılması

            var result = await userService.CreateUserAsync(user);

            // assert karşılaştırma işlemleri

            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.Name, result.Name);

        }


        [Theory]
        [InlineData(1, "user1")]
        [InlineData(2, "user2")]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists(int id, string name)
        {
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var userService = new UserService(context);


            var user = new User { Id = id, Name = name };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            //Act fonksiyonun çağrılması

            var result = await userService.GetUserByIdAsync(id);


            //assert karşılaştırma işlemleri 

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(name, result.Name);


        }




    }
}
