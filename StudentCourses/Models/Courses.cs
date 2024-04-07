using System.ComponentModel.DataAnnotations;

namespace StudentCourses.Models
{
    public class Courses
    {
        [Key]
        public Guid? CourseId{ get; set; }
        public string? CourseName { get; set; }
        public string? Description { get; set; }
        public List<StudentCourses>? StudentCourses { get; set; }
    }
}
