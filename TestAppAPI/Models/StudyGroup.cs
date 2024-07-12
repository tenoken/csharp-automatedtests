using System.ComponentModel.DataAnnotations;

namespace TestAppAPI.Models
{
    public class StudyGroup
    {
        public StudyGroup()
        {
            CreationDate = DateTime.Now;
        }

        [MaxLength(30, ErrorMessage = "The name cannot be greater than 30 characters")]
        [MinLength(5, ErrorMessage = "The name cannot be lower than 5 characters")]
        public string Name { get; set; }
        public SubjectEnum Subject { get; set; }
        public DateTime CreationDate { get; internal set; }
        public int SubjectId { get; internal set; }
        public int Id { get; internal set; }

        public enum SubjectEnum
        {
            Math = 1,
            Chemistry = 2,
            Physics = 3
        }
    }
}
