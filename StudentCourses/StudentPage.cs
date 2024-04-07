using StudentCourses.Models;

namespace StudentCourses
{
    public class StudentPage
    {
        public List<Student> Students { get; set; } = new List<Student>();
        public int Pages { get; set; }  
        public int CurrentPage { get; set; }
    }
}
