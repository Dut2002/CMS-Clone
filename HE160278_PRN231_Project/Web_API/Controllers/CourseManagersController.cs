using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API.Models;

namespace Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CourseManagersController : ControllerBase
	{
		private readonly ApplicationDBContext _context;
		public CourseManagersController(ApplicationDBContext context)
		{
			_context = context;
		}

		[HttpGet("GetCourseManager")]
		public IActionResult GetCourseManager(int courseId, int userId)
		{
			try
			{
				var manager = _context.CourseManagers.FirstOrDefault(m => m.CourseId == courseId && m.UserId == userId);
				if (manager == null)
				{
					return NotFound();
				}
				else
					return Ok(manager);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("GetCourseManagerByCourse")]
		public IActionResult GetCourseManagerByCourse(int courseId)
		{
			try
			{
				var course = _context.Courses.Include(c => c.CourseManagers).ThenInclude(c => c.User).FirstOrDefault(c => c.CourseId == courseId);
				return Ok(course.CourseManagers.Where(m => course.CreatorId != m.UserId).Select(manager => new
				{
					manager.CourseManagerId,
					manager.UserId,
					manager.CourseId,
					User = manager.User.FirstName + " " + manager.User.LastName,
					manager.IsStaff
				}));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("CreateCourseManager")]
		public IActionResult CreateCourseManager([FromQuery] int courseId, [FromQuery] int userId)
		{
			try
			{
				var manager = _context.CourseManagers.FirstOrDefault(m => m.CourseId == courseId && m.UserId == userId);
				if (manager == null)
				{
					manager = new CourseManager
					{
						CourseId = courseId,
						UserId = userId
					};
					_context.CourseManagers.Add(manager);
					_context.SaveChanges();
				}
				else if (manager.IsStaff.HasValue && !manager.IsStaff.Value)
				{
					manager.IsStaff = null;
					_context.CourseManagers.Update(manager);
					_context.SaveChanges();

				}
				return Ok(manager);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("UpdateCourseManager")]
		public IActionResult UpdateCourseManager([FromQuery] int courseManagerId, [FromQuery] bool? isStaff)
		{
			try
			{
				var manager = _context.CourseManagers.FirstOrDefault(m => m.CourseManagerId == courseManagerId);
				if (manager == null)
				{
					return NotFound();
				}
				manager.IsStaff = isStaff;
				_context.CourseManagers.Update(manager);
				_context.SaveChanges();
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("UpdateCourseManagerAll")]
		public IActionResult UpdateCourseManagerAll([FromQuery] int courseId, [FromQuery] bool isStaff)
		{
			try
			{
				var managers = _context.CourseManagers.Where(m => m.CourseId == courseId && m.IsStaff == null).ToList();
				if (managers.Any())
				{
					foreach (var manager in managers)
					{
						manager.IsStaff = isStaff;
					}
					_context.CourseManagers.UpdateRange(managers);
					_context.SaveChanges();
				}
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
