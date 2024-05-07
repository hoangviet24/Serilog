using Microsoft.EntityFrameworkCore;
using StudentCourses.Data;
using StudentCourses.Models;

namespace StudentCourses.Services
{
    public class CoursesServices:ICoursesServices
    {
        private readonly DataContext _context;
        public CoursesServices(DataContext context) {  _context = context; }

        #region Courses

        public async Task<List<Courses>> GetAllCourses()
        {
            try
            {
                return await _context.Courses.ToListAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Courses> GetIdCourses(Guid id, bool includeCourses)
        {
            try
            {
                if (includeCourses)
                {
                    return await _context.Courses.Include(c => c.StudentCourses).FirstOrDefaultAsync(i => i.CourseId == id);
                }

                return await _context.Courses.FindAsync(id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Courses> AddCourses(Courses courses)
        {
            try
            {
                await _context.Courses.AddAsync(courses);
                await _context.SaveChangesAsync();
                return await _context.Courses.FindAsync(courses.CourseId);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Courses> UpdateCourses(Courses courses)
        {
            try
            {
                _context.Entry(courses).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return courses;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteCourses(Courses courses)
        {
            try
            {
                var dbCourses = await _context.Courses.FindAsync(courses.CourseId);
                if (dbCourses == null)
                {
                    return (false, "Not Found");
                }

                _context.Courses.Remove(courses);
                _context.SaveChanges();
                return (true, "Success");
            }
            catch (Exception e)
            {
                return (false, "Failed");
            }
        }

        

        #endregion

        #region Student
        public async Task<List<Student>> getAllStudent()
        {
            try
            {
                return await _context.Students.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<Student> GetIdStudent(Guid id, bool includeCourses)
        {
            try
            {
                if (includeCourses)
                {
                    return await _context.Students.Include(c => c.StudentCourses).FirstOrDefaultAsync(i => i.StudentId== id);
                }

                return await _context.Students.FindAsync(id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Student> AddStudent(Student student)
        {
            try
            {
                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();
                return await _context.Students.FindAsync(student.StudentId);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Student> UpdateStudent(Student student)
        {
            try
            {
                _context.Entry(student).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return student;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteStudent(Student student)
        {
            try
            {
                var dbStudent = await _context.Students.FindAsync(student.StudentId);
                if (dbStudent == null)
                {
                    return (false, "Courses could not be found");
                }

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return (true, "Amzing good job you");
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        }
        #endregion
    }
}
