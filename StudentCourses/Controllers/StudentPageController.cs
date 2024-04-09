using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCourses.Data;
using StudentCourses.Models;
using Serilog;
namespace StudentCourses.Controllers
{
    public class StudentPageController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public StudentPageController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("{page}")]
        public async Task<ActionResult<List<Student>>> GetStudent(int page)
        {
            if(_dataContext.Students == null)
            {
                return NotFound();
            }
            var pageResults = 2f;
            var pageCount = Math.Ceiling(_dataContext.Students.Count()/pageResults);
            var student = await _dataContext.Students
                .Skip((page-1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();
            var response = new StudentPage
            {
                Students = student,
                CurrentPage = page,
                Pages = (int)pageCount,
            };
            Log.Information("Student Page => {@response}", response);
            return Ok(response);
        }
    }
}
