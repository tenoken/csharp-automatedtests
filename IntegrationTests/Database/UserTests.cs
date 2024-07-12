using TestAppAPI.Data.Respositories;
using TestAppAPI.Models;

namespace IntegrationTests.Database
{
    [TestFixture(Category = "Integration.User")]
    internal class UserTests
    {
        private UserRepository _userRepository;

        private User _result;

        [SetUp]
        public void Setup()
        {
            _userRepository = new UserRepository();
        }

        [Test]
        public async Task Given_User_When_Valid_Should_BeInserted()
        {
            // Arrange
            var user = new User
            {
                Id = 0,
                Name = "John Smith",
            };
            var sut = _userRepository;

            // Act
            Task task = sut.AddUser(user);
            _result = await sut.GetByName(user.Name);

            // Assert
            Assert.True(task.IsCompletedSuccessfully);
            Assert.IsNotNull(_result);
            Assert.AreEqual(user.Name, _result.Name);
        }

        [TearDown]
        public async Task Teardown()
        {
            await _userRepository.DeleteUser(_result.Id, _result);
            _userRepository.Dispose();
        }
    }
}
