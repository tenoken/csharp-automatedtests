using TestAppAPI.Data.Respositories.Interfaces;
using TestAppAPI.Models;
using static TestAppAPI.Models.StudyGroup;

namespace TestAppAPI.Data.Respositories
{
    public class StudyGroupRepository : DataContext, IStudyGroupRepository
    {
        private static IUserRepository _userRepository;

        public StudyGroupRepository(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public Task<StudyGroup> CreateStudyGroup(StudyGroup studyGroup)
        {
            return Task.Run(() =>
            {
                var subId = (int)studyGroup.Subject;
                var subject = Query<int>("SELECT Id FROM Subjects WHERE ID = @Id", new { Id = subId }).First();
                studyGroup.SubjectId = subject;
                // only one group for a single subject
                //if (Query<StudyGroup>("SELECT TOP 1 * FROM StudyGroups WHERE SubjectId = @Id", new { Id = subId }).Count() > 0)
                //    throw new Exception("There is already a group for selected subject.");
                // cannot have same name
                if (Query<StudyGroup>("SELECT TOP 1 * FROM StudyGroups WHERE Name = @Name", new { Name = studyGroup.Name }).Count() > 0)
                    throw new Exception("There is already a group with the provided name.");

                Execute("INSERT INTO StudyGroups (SubjectId, Name, CreationDate) VALUES (@SubjectId, @Name, @CreationDate)", studyGroup);
                return studyGroup;
            });
        }

        public Task<List<StudyGroup>> GetStudyGroups()
        {
            return Task.Run(() =>
            {
                var groups = Query<DataObjects.StudyGroup>("SELECT * FROM StudyGroups");
                var list = new List<StudyGroup>();
                foreach (var group in groups)
                {
                    list.Add(new StudyGroup() { Name = group.Name, Subject = (SubjectEnum)group.SubjectId, SubjectId = group.SubjectId, Id = group.Id, CreationDate = group.CreationDate });
                }
                return list;
            });
        }

        public Task JoinStudyGroup(int studyGroupId, int userId)
        {
            return Task.Run(() =>
            {
                if (Query<DataObjects.StudyGroup>("SELECT TOP 1 * FROM UserStudyGroups WHERE UserId = @UserId AND StudyGroupId = @StudyGroupId",
                    new { Userid = userId, StudyGroupId = studyGroupId }).Count() == 0)
                    Execute("INSERT INTO UserStudyGroups (UserId, StudyGroupId) VALUES (@UserId, @StudyGroupId)", new { UserId = userId, StudyGroupId = studyGroupId });
                else
                    throw new InvalidOperationException("User already joined this study group.");
            });
        }

        public Task LeaveStudyGroup(int studyGroupId, int userId)
        {
            return Task.Run(() =>
            {
                if (Query<DataObjects.StudyGroup>("SELECT TOP 1 * FROM UserStudyGroups WHERE UserId = @UserId AND StudyGroupId = @StudyGroupId",
                    new { Userid = userId, StudyGroupId = studyGroupId }).Count() == 1)
                    Execute("DELETE FROM UserStudyGroups WHERE UserId = @UserId AND StudyGroupId = @StudyGroupId", new { UserId = userId, StudyGroupId = studyGroupId });
                else
                    throw new InvalidOperationException("User already left this study group.");
            });
        }

        public Task<List<StudyGroup>> SearchStudyGroups(string subject)
        {
            return Task.Run(() =>
            {
                var groups = Query<DataObjects.StudyGroup>(@"SELECT
	                                                            G.*
                                                            FROM
                                                                StudyGroups G
                                                            INNER JOIN
	                                                            Subjects S
                                                            ON G.SubjectId = S.id
                                                            WHERE S.Name LIKE @Subject + '%'", new { Subject = subject });
                var list = new List<StudyGroup>();
                foreach (var group in groups)
                {
                    list.Add(new StudyGroup() { Name = group.Name, Subject = (SubjectEnum)group.SubjectId, SubjectId = group.SubjectId, Id = group.Id, CreationDate = group.CreationDate });
                }
                return list;
            });
        }

        public Task DeleteStudyGroup(int id, StudyGroup studyGroup)
        {
            return Task.Run(() =>
            {
                if (studyGroup.Id != id)
                    throw new ArgumentException("Id and study group are different.");

                Execute("DELETE FROM StudyGroups WHERE Id = @Id", new { Id = id });
            });
        }

        public Task<List<StudyGroup>> SearchStudyGroups(DateTime dateTime)
        {
            return Task.Run(() =>
            {
                var groups = Query<DataObjects.StudyGroup>(@"SELECT
	                                                            G.*
                                                            FROM
                                                                StudyGroups G
                                                            INNER JOIN
	                                                            Subjects S
                                                            ON G.SubjectId = S.id
                                                            WHERE G.CreationDate >= @DateTime", new { DateTime = dateTime.ToString("yyyy-MM-dd") });
                var list = new List<StudyGroup>();
                foreach (var group in groups)
                {
                    list.Add(new StudyGroup() { Name = group.Name, Subject = (SubjectEnum)group.SubjectId, SubjectId = group.SubjectId, Id = group.Id, CreationDate = group.CreationDate });
                }
                return list;
            });
        }
    }
}
