using Microsoft.AspNetCore.Mvc;
using Serilog;
using StudentCourses.Models;
using StudentCourses.Services;

namespace REST_API_TEMPLATE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ICoursesServices _coursesService;

        public StudentController(ICoursesServices coursesService)
        {
            _coursesService = coursesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetIdStudent()
        {
            var students = await _coursesService.getAllStudent();

            if (students == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No authors in database");
            }
            Log.Information("Student => {@students}", students);
            return StatusCode(StatusCodes.Status200OK, students);
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetIdStudent(Guid id, bool includeCourses = false)
        {
            Student student = await _coursesService.GetIdStudent(id, includeCourses);

            if (student == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Author found for id: {id}");
            }
            Log.Information("Student => {@student}", student);
            return StatusCode(StatusCodes.Status200OK, student);
        }

        [HttpPost]
        public async Task<ActionResult<Courses>> AddStudent(Student student)
        {
            var dbStudent = await _coursesService.AddStudent(student);

            if (dbStudent == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{student.Name} could not be added.");
            }
            Log.Information("Student => {@dbStudent}", dbStudent);
            return CreatedAtAction("GetIdStudent", new { id = student.StudentId }, student);
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateStudent(Guid id, Student student)
        {
            if (id != student.StudentId)
            {
                return BadRequest();
            }

            Student dbStudent = await _coursesService.UpdateStudent(student);

            if (dbStudent == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{student.Name} could not be updated");
            }
            Log.Information("Student => {@dbStudent}", dbStudent);
            return NoContent();
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _coursesService.GetIdStudent(id, false);
            (bool status, string message) = await _coursesService.DeleteStudent(student);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
            Log.Information("Student => {@student}", student);
            return StatusCode(StatusCodes.Status200OK, student);
        }
    }
}