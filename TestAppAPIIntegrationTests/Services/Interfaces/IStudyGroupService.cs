using RestSharp;

namespace TestAppAPIIntegrationTests.Services.Interfaces
{
    public interface IStudyGroupService
    {
        RestResponse CreateStudyGroup(string json);
        RestResponse GetStudyGroups();
        RestResponse JoinStudyGroup(int userId, int studyGroupId);
        RestResponse LeaveStudyGroup(int userId, int studyGroupId);
        RestResponse SearchGroupByDate(DateTime convertedDate, bool sorted);
        RestResponse SearchGroupBySubject(string subject);
    }
}
