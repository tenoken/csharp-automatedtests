using BoDi;
using TestAppAPIIntegrationTests.Database.SqlServer;
using TestAppAPIIntegrationTests.Database.SqlServer.Interfaces;
using TestAppAPIIntegrationTests.Services;
using TestAppAPIIntegrationTests.Services.Interfaces;

namespace TestAppAPIIntegrationTests.StepDefinitions
{
    [Binding]
    internal class Hooks
    {
        private readonly IObjectContainer _objectContainer;

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeTestRun]
        public static void SetUp(IObjectContainer objectContainer)
        {
            objectContainer.RegisterTypeAs<SqlServer, ISqlServer>();
            objectContainer.RegisterTypeAs<StudyGroupService, IStudyGroupService>();
        }
    }
}
