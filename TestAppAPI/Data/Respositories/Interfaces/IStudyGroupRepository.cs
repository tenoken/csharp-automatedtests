using TestAppAPI.Models;

namespace TestAppAPI.Data.Respositories.Interfaces
{
    public interface IStudyGroupRepository
    {
        Task<StudyGroup> CreateStudyGroup(StudyGroup studyGroup);
        Task<List<StudyGroup>> GetStudyGroups();
        Task JoinStudyGroup(int studyGroupId, int userId);
        Task LeaveStudyGroup(int studyGroupId, int userId);
        Task<List<StudyGroup>> SearchStudyGroups(string subject);
        Task<List<StudyGroup>> SearchStudyGroups(DateTime dateTime);
    }
}
