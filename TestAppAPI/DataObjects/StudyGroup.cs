using static TestAppAPI.Models.StudyGroup;

namespace TestAppAPI.DataObjects
{
    public record StudyGroup
    {
        public string Name;
        public int SubjectId;
        public SubjectEnum Subject;
        public DateTime CreationDate;
        public int Id;
    }
}
