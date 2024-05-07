using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using StudentCourses.Models;
using StudentCourses.Services;

namespace REST_API_TEMPLATE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICoursesServices _coursesService;

        public CoursesController(ICoursesServices coursesService)
        {
            _coursesService = coursesService;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _coursesService.GetAllCourses();

            if (courses == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No authors in database");
            }
            Log.Information("Courses => {@courses}", courses);
            return StatusCode(StatusCodes.Status200OK, courses);
        }

        [HttpGet("id"),Authorize]
        public async Task<IActionResult> GetCourses(Guid id, bool includeCourses = false)
        {
            Courses courses = await _coursesService.GetIdCourses(id, includeCourses);

            if (courses == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Author found for id: {id}");
            }
            Log.Information("Courses => {@courses}", courses);
            return StatusCode(StatusCodes.Status200OK, courses);
        }

        [HttpPost,Authorize]
        public async Task<ActionResult<Courses>> AddCourses(Courses courses)
        {
            var dbCourses = await _coursesService.AddCourses(courses);

            if (dbCourses == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{courses.CourseName} could not be added.");
            }
            Log.Information("Courses => {@dbCourses}", dbCourses);
            return CreatedAtAction("GetCourses", new { id = courses.CourseId }, courses);
        }

        [HttpPut("id"),Authorize]
        public async Task<IActionResult> UpdateCourses(Guid id, Courses courses)
        {
            if (id != courses.CourseId)
            {
                return BadRequest();
            }

            Courses dbCourses = await _coursesService.UpdateCourses(courses);

            if (dbCourses == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{courses.CourseName} could not be updated");
            }
            Log.Information("Courses => {@dbCourses}", dbCourses);
            return NoContent();
        }

        [HttpDelete("id"), Authorize]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var courses = await _coursesService.GetIdCourses(id, false);
            (bool status, string message) = await _coursesService.DeleteCourses(courses);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
            Log.Information("Courses => {@courses}", courses);
            return StatusCode(StatusCodes.Status200OK, courses);
        }
    }
}