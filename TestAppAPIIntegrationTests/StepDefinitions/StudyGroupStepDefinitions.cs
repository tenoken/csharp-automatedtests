using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using TestAppAPIIntegrationTests.Database.SqlServer.Interfaces;
using TestAppAPIIntegrationTests.Endpoints;

namespace TestAppAPIIntegrationTests.StepDefinitions
{
    [Binding]
    public class StudyGroupStepDefinitions
    {
        private readonly StudyGroupEndpoint _endpoint;
        private readonly ISqlServer _sqlServer;
        private RestResponse _result;

        public StudyGroupStepDefinitions(StudyGroupEndpoint endpoint, ISqlServer sqlServer)
        {
            _endpoint = endpoint;
            _sqlServer = sqlServer;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _endpoint.CleanUpSubjects();
        }

        [Given(@"there is no group created for the avaiable subjects")]
        public void GivenThereIsNoGroupCreatedForTheAvaiableSubjects()
        {
            //_endpoint.CleanUpSubjects();
        }


        [When(@"I set the name (.*) and subject (.*)")]
        public void GivenISetTheNameInrationalAndSubjectMath(string name, string subject)
        {
            _endpoint.SetName(name);
            _endpoint.SetSubject(subject);
        }

        [When(@"I send the request")]
        public void WhenISendTheRequest()
        {
            _result = _endpoint.SendRequest();
            var x = _endpoint.GetAffectedRows();
        }

        [Then(@"I receive the status (.*) from the server")]
        public void ThenIReceiveTheStatusFromTheServer(int status)
        {
            Assert.AreEqual(status, (int)_result.StatusCode);
        }

        [Then(@"error message is equal to (.*)")]
        public void ThenErrorMessageIsEqualTo(string message)
        {
            var result = JsonConvert.DeserializeObject<dynamic>(_result.Content);
            Assert.AreEqual(message, result.message.ToString());
        }

        [Then(@"I check if the creation date was recorded")]
        public void ThenICheckIfTheCreationDateWasRecorded()
        {
            if (_endpoint.GetAffectedRows().Count() == 1)
            {
                var row = _endpoint.GetAffectedRows().First();
                Assert.IsNotNull(row.CreationDate);
                Assert.AreEqual(DateTime.Now.Date, (DateTime)row.CreationDate.Date);
            }
            else
                Assert.Fail("Number of affected rows is different from the expected.");
        }

        [Then(@"I check if it successfully added to database")]
        public void ThenIChekIfItSuccessfullyAddedToDatabase()
        {
            var row = _endpoint.GetAffectedRows();
            Assert.IsNotNull(row);
            Assert.AreEqual(1, row.Count());
        }

        [Given(@"the study groups are already created")]
        public void GivenTheStudyGroupsAreAlreadyCreated()
        {
            _endpoint.SetUpStudyGroups();
        }

        [Given(@"I am a valid user (.*)")]
        public void GivenIAmAValidUser(string name)
        {
            _endpoint.SetUpUser(name);
        }

        [Given(@"the study group about (.*) I want to join exists")]
        public void GivenTheStudyGroupAboutChemestryIWantToJoinExists(string subject)
        {
            _endpoint.CheckStudyGroup(subject);
        }

        [When(@"choose to join the study group")]
        public void WhenChooseToJoinTheStudyGroup()
        {
            _result = _endpoint.JoinToStudyGroup();
        }

        [When(@"I search for groups by subject (.*)")]
        public void WhenISearchForGroupsBySubject(string subject)
        {
            _result = _endpoint.SearchGroups(subject);
        }

        [Then(@"reponse has subject equals to (.*)")]
        public void ThenReponseHasSubjectEqualsTo(string subject)
        {
            var json = _endpoint.ConvertToDynamic(_result.Content);
            Assert.IsNotNull(json);
            Assert.That(json.All(x => x.Subject == subject));
        }

        [Given(@"the study groups are already created from (.*) with (.*) days past")]
        public void GivenTheStudyGroupsAreAlreadyCreatedWithCustomValues(string from, int daysPast)
        {
            var counter = 1;
            var date = DateTime.Parse(from);
            var group = new StudyGroup { Subject = "Chemistry", Name = $"Test {counter}", CreationDate = date };

            for (int i = 0; i < daysPast; i++)
            {
                date = date.AddDays(1);
                if (date.Day == 1)
                {
                    break;
                }

                _sqlServer.Execute("INSERT INTO StudyGroups (SubjectId, Name, CreationDate) VALUES(@SubjectId , @Name , @Date)",
                new { SubjectId = 2, Name = $"Test {counter}", Date = date });
                counter++;
                var rows = _endpoint.GetAffectedRows(group);

                if (rows.Count() == 0)
                    throw new Exception($"Precondition failed: The subject {group.Subject} could not be created.");
            }

        }

        [When(@"I search for groups by creation date (.*) and sorted (.*)")]
        public void WhenISearchForGroupsByCreationDate(string date, bool sorted)
        {
            var convertedDate = Convert.ToDateTime(date);
            _result = _endpoint.SearchGroups(convertedDate, sorted);
        }

        [Then(@"reponse has date equals to (.*)")]
        public void ThenReponseHasDateEqualsTo(string date)
        {
            throw new PendingStepException();
        }

        [Given(@"I am an user (.*) that alredy is part of a study group (.*)")]
        public void GivenIAmAnUserWalterWhiteThatAlredyIsPartOfAStudyGroup(string user, string subject)
        {
            _endpoint.SetUpUser(user);
            _endpoint.SetUpStudyGroups();
            _endpoint.SetUpUserJoinedStudyGroup(user, subject);
        }

        [When(@"choose to leave the study group")]
        public void WhenChooseToLeaveTheStudyGroup()
        {
            _result = _endpoint.LeaveStudyGroup();
        }

        [When(@"I check the avaiable study groups")]
        public void WhenICheckTheAvaiableStudyGroups()
        {
            _result = _endpoint.GetStudyGroups();
        }

        [Then(@"the number of avaiable study groups is equal to (.*)")]
        public void ThenTheNumberOfAvaiableStudyGroupsIsEqualTo(int groupsCount)
        {
            var items = _endpoint.ConvertToDynamic(_result.Content);
            Assert.AreEqual(groupsCount, items.Count());
        }

        [Then(@"validate the expected date (.*)")]
        public void ThenValidateTheSortedList(string expecteDate)
        {
            var items = _endpoint.ConvertToDynamic(_result.Content);
            Assert.AreEqual(items.First().CreationDate.ToString("yyyy-MM-dd"), expecteDate);
        }

        [AfterScenario()]
        public void TearDown()
        {
            _endpoint.TearDown();
        }

        [AfterScenario("@user")]
        public void TearDonw()
        {
            _endpoint.CleanGroupJoinAction();
            _endpoint.CleanUpUser();
            _endpoint.CleanStudyGroups();
        }
    }
}
