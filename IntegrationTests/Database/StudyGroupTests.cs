using TestAppAPI.Data.Respositories;
using static TestAppAPI.Models.StudyGroup;
using StudyGroup = TestAppAPI.Models.StudyGroup;

namespace IntegrationTests.Database
{
    [TestFixture(Category = "Integration.StudyGroup")]
    internal class StudyGroupTests
    {
        private StudyGroupRepository _studyGroupRepository;

        private List<StudyGroup> _result;

        [SetUp]
        public void Setup()
        {
            _studyGroupRepository = new StudyGroupRepository(new UserRepository());
        }

        [Test]
        public async Task Given_Something_Should_CreateSudyGroup_When_Invoked()
        {
            // Arrange
            var studyGroup = new StudyGroup
            {
                CreationDate = DateTime.Now,
                Name = "Study Group Test",
                Subject = SubjectEnum.Math,
            };

            // Act
            var sut = await _studyGroupRepository.CreateStudyGroup(studyGroup);
            _result = await _studyGroupRepository.SearchStudyGroups(studyGroup.Subject.ToString());

            // Assert            
            Assert.IsNotNull(_result);
            Assert.AreEqual(studyGroup.Name, _result.First().Name);
        }

        [TearDown]
        public async Task TeardownAsync()
        {
            await _studyGroupRepository.DeleteStudyGroup(_result.First().Id, _result.First());
            _studyGroupRepository.Dispose();
        }
    }
}
