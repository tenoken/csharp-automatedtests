using Newtonsoft.Json;
using RestSharp;
using System.Collections.ObjectModel;
using TestAppAPIIntegrationTests.Database.SqlServer.Interfaces;
using TestAppAPIIntegrationTests.Services.Interfaces;

namespace TestAppAPIIntegrationTests.Endpoints
{
    public class StudyGroupEndpoint
    {
        private readonly IStudyGroupService _studyGroupService;
        private readonly ISqlServer _sqlServer;
        private StudyGroup body;
        private Collection<dynamic> _studyGroups;
        private Collection<dynamic> _users;
        private string _subject;
        const string WIPE_STUDYGROUP_TABLE = "DELETE FROM StudyGroups";
        const string DELETE_STUDYGROUP_BY_ID = "DELETE FROM StudyGroups WHERE Id = @Id";

        const string INSERT_USER = "INSERT INTO Users (Name) VALUES (@Name)";
        const string DELETE_USER = "DELETE FROM Users WHERE Id = @Id";
        const string QUERY_USER = "SELECT * FROM Users WHERE Name = @Name  ORDER BY Id DESC";
        const string DELETE_USER_STUDYGROUP_TABLE = "DELETE FROM UserStudyGroups";
        const string QUERY_STUDYGROUP_TABLE = @"SELECT
	                                                G.*
                                                FROM
                                                    StudyGroups G
                                                INNER JOIN
	                                                Subjects S
                                                ON G.SubjectId = S.id
                                                WHERE S.Name LIKE @Subject + '%'";

        public StudyGroupEndpoint(IStudyGroupService sutudyGroupService, ISqlServer sqlServer)
        {
            _studyGroupService = sutudyGroupService;
            _sqlServer = sqlServer;
            body = new StudyGroup();
            _studyGroups = new Collection<dynamic>();
            _users = new Collection<dynamic>();
        }

        internal void CleanUpSubjects()
        {
            _sqlServer.Execute(WIPE_STUDYGROUP_TABLE);
        }

        internal RestResponse SendRequest()
        {
            return _studyGroupService.CreateStudyGroup(JsonConvert.SerializeObject(body));
        }

        internal void SetName(string name)
        {
            body.Name = name;
        }

        internal void SetSubject(string subject)
        {
            body.Subject = subject;
        }

        internal IEnumerable<dynamic> GetAffectedRows(StudyGroup studyGroup = null)
        {
            if (studyGroup is not null)
                body = studyGroup;

            var rows = _sqlServer.Query<dynamic>(QUERY_STUDYGROUP_TABLE, body);

            foreach (var row in rows)
            {
                if (!_studyGroups.Any(c => c.Id == row.Id))
                    _studyGroups.Add(row);
            }

            return rows;
        }

        internal void TearDown()
        {
            // Should run specific teardown
            if (_users.Count > 0)
                return;

            CleanStudyGroups();
        }

        internal void SetUpStudyGroups()
        {
            var mathGroup = new StudyGroup() { Name = "Math Group", Subject = "Math" };
            var chemistryGroup = new StudyGroup() { Name = "Chemistry Group", Subject = "Chemistry" };
            var PhysicsGroup = new StudyGroup() { Name = "Physics Group", Subject = "Physics" };

            var groups = new List<StudyGroup>();
            groups.Add(mathGroup);
            groups.Add(chemistryGroup);
            groups.Add(PhysicsGroup);

            foreach (var group in groups)
            {
                _studyGroupService.CreateStudyGroup(JsonConvert.SerializeObject(group));
                var rows = GetAffectedRows(group);
                if (rows.Count() == 1)
                    continue;
                else
                    throw new Exception($"Precondition failed: The subject {group.Subject} could not be created.");
            }
        }

        internal void SetUpUser(string name)
        {
            // if there any row it means the test still running, then return
            var rows = _sqlServer.Query<dynamic>(QUERY_USER, new { Name = name });
            if (rows.Count() > 0)
            {
                _users.Add(rows.First());
                return;
            }

            _sqlServer.Execute(INSERT_USER, new { Name = name });
            var user = _sqlServer.Query<dynamic>(QUERY_USER, new { Name = name }).First();
            _users.Add(user);
        }

        internal void CleanUpUser()
        {
            foreach (var user in _users)
            {
                var rows = _sqlServer.Query<dynamic>(QUERY_USER, new { Name = user.Name });

                if (rows.Count() == 1)
                    _sqlServer.Execute(DELETE_USER, new { Id = rows.First().Id });
                else
                    throw new Exception($"Precondition failed: The user {user.Name} could not be deleted.");
            }
        }

        internal void CleanGroupJoinAction()
        {
            _sqlServer.Execute(DELETE_USER_STUDYGROUP_TABLE);
        }

        internal void CheckStudyGroup(string subject)
        {
            _subject = subject;
            if (_sqlServer.Query<dynamic>(QUERY_STUDYGROUP_TABLE, new { Subject = subject }).Count() > 0)
                return;
            else
                throw new Exception($"The study group with subject {subject} doe not exist");
        }

        internal RestResponse JoinToStudyGroup()
        {
            var user = _users.First();
            var group = _studyGroups.First(s => s.Name.Contains(_subject));
            int userId = Convert.ToInt32(user.Id);
            int groupId = Convert.ToInt32(group.Id);
            return _studyGroupService.JoinStudyGroup(userId, groupId);//JsonConvert.SerializeObject(new { userId = userId, studyGroupId = groupId })
        }

        internal void CleanStudyGroups()
        {
            foreach (var group in _studyGroups)
            {
                _sqlServer.Execute(DELETE_STUDYGROUP_BY_ID, group);
            }
        }

        internal void SetUpUserJoinedStudyGroup(string user, string subject)
        {
            _subject = subject;
            if ((int)JoinToStudyGroup().StatusCode != 200)
                throw new Exception($"It could not possible to add user {user} to study group about {subject}");
        }

        internal RestResponse LeaveStudyGroup()
        {
            var user = _users.First();
            var group = _studyGroups.First(s => s.Name.Contains(_subject));
            int userId = Convert.ToInt32(user.Id);
            int groupId = Convert.ToInt32(group.Id);
            return _studyGroupService.LeaveStudyGroup(userId, groupId);
        }

        internal RestResponse GetStudyGroups()
        {
            return _studyGroupService.GetStudyGroups();
        }

        internal List<StudyGroup> ConvertToDynamic(string content)
        {
            return JsonConvert.DeserializeObject<List<StudyGroup>>(content);
        }

        internal RestResponse SearchGroups(string subject)
        {
            return _studyGroupService.SearchGroupBySubject(subject);
        }

        internal RestResponse SearchGroups(DateTime convertedDate, bool sorted)
        {
            return _studyGroupService.SearchGroupByDate(convertedDate, sorted);
        }
    }

    internal class StudyGroup
    {
        public string Name { get; set; }
        public string Subject { get; set; }

        public DateTime CreationDate;
        public int Id;
    }
}
