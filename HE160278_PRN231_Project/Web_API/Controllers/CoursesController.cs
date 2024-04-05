using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API.Models;
using Web_API.Models.Dto;

namespace Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CoursesController : ControllerBase
	{
		private readonly ApplicationDBContext _context;
		public CoursesController(ApplicationDBContext context)
		{
			_context = context;
		}

		[HttpGet("GetCourses")]
		public ActionResult GetCourses(string? code, string? search, int? userId)
		{
			try
			{
				var coursesQuery = _context.Courses
					.Include(c => c.Subject) // Nối bảng Course với Subject
					.Include(c => c.Creator) // Nối bảng Course với Subject
					.Include(c => c.CourseManagers) // Nối bảng Course với CourseManagers
					.ThenInclude(cm => cm.User)
					// Nối bảng CourseManagers với User
					.AsQueryable();

				// Kiểm tra và áp dụng điều kiện cho biến code nếu không null
				if (!string.IsNullOrEmpty(code))
				{
					coursesQuery = coursesQuery.Where(c => c.Code.ToLower() == code.ToLower());
				}

				if (userId != null)
				{
                    coursesQuery = coursesQuery.Where(c => c.CourseManagers.Any(cm => cm.UserId == userId && cm.IsStaff == true));
                }

				// Kiểm tra và áp dụng điều kiện cho biến search nếu không null
				if (!string.IsNullOrEmpty(search))
				{
					coursesQuery = coursesQuery.Where(c => c.CourseName.ToLower().Contains(search.ToLower()) || c.Code.ToLower().Contains(search.ToLower()));
				}

				var courses = coursesQuery.ToList();

				if (!courses.Any())
				{
					return NotFound();
				}
				else
				{
					return Ok(courses.Select(c => new
					{
						c.CourseId,
						c.Code,
						c.CourseName,
						c.Semester,
						c.Year,
						c.CreatorId,
						Creator = c.Creator.FirstName +" "+ c.Creator.LastName,
						SubjectName = c.Subject.Name,
						Managers = c.CourseManagers.Select(cm => new
						{
							cm.UserId,
							cm.User.FirstName,
							cm.User.LastName
						})
					}));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("GetCourse")]
		public ActionResult GetCourse(int courseId)
		{
			try
			{
				var course = _context.Courses
					.Include(c => c.Subject) // Nối bảng Course với Subject
					.Include(c => c.Creator) // Nối bảng Course với Subject
					.Include(c => c.CourseManagers) // Nối bảng Course với CourseManagers
					.ThenInclude(cm => cm.User) // Nối bảng CourseManagers với User
					.FirstOrDefault(c => c.CourseId == courseId );


				if (course == null)
				{
					return NotFound();
				}
				else
				{
					return Ok( new
					{
						course.CourseId,
						course.Code,
						course.CourseName,
						course.Semester,
						course.Year,
						Creator = course.Creator.FirstName + " " + course.Creator.LastName,
						SubjectName = course.Subject.Name,
						Managers = course.CourseManagers.Select(cm => new
						{
							cm.UserId,
							cm.User.FirstName,
							cm.User.LastName
						})
					});
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("CreateCourse")]
		public ActionResult Create(Course course)
		{
			try
			{
				var week = GetWeek(course.StartDate, course.Enđate);

				// Thêm khóa học vào cơ sở dữ liệu
				_context.Courses.Add(course);
				_context.SaveChanges();

				// Lấy CourseId của khóa học vừa được tạo
				var courseId = course.CourseId;

				// Thêm các ContentCourse với Title từ danh sách week và CourseId đã lấy được
				foreach (var weekTitle in week)
				{
					var contentCourse = new ContentCourse
					{
						CourseId = courseId,
						Title = weekTitle
					};
					_context.ContentCourses.Add(contentCourse);
				}

				// Lưu thay đổi vào cơ sở dữ liệu
				_context.SaveChanges();

				return Ok("Success Create");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		private List<string> GetWeek(DateTime startDate, DateTime endDate)
		{
			List<string> weeks = new List<string>();

			// Set the initial start date of the current week to the start date provided
			DateTime currentWeekStart = startDate;

			// Loop until the current week start date is before or equal to the end date
			while (currentWeekStart <= endDate)
			{
				// Determine the end date of the current week
				DateTime currentWeekEnd = currentWeekStart.AddDays(6); // Add 6 days to get the end of the week

				// If the end date of the current week exceeds the end date provided, adjust it
				if (currentWeekEnd > endDate)
				{
					currentWeekEnd = endDate; // Set the current week end date to the end date provided
				}

				// Add the current week's start and end dates to the list of weeks
				weeks.Add($"{currentWeekStart.ToShortDateString()} - {currentWeekEnd.ToShortDateString()}");

				// Move to the start date of the next week
				currentWeekStart = currentWeekEnd.AddDays(1); // Add 1 day to get the start of the next week
			}

			return weeks;
		}


		[HttpGet("GetCourseById")]
		public ActionResult GetById(int courseId, int creatorId)
		{
			try
			{
				var course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId && c.CreatorId == creatorId);
				if (course == null)
				{
					return NotFound();
				}
				return Ok(new
				{
					course.CourseId,
					course.Code,
					course.CourseName,
					course.Semester,
					course.Year,
					course.CreatorId
				});
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("GetCourseWithContent")]
		public ActionResult GetƯithContent(int courseId, int userId)
		{
			try
			{
				var courseManager = _context.CourseManagers.Where(cm => cm.CourseId == courseId && cm.UserId == userId).ToList();
				if (!courseManager.Any())
				{
					return Unauthorized();
				}
				var course = _context.Courses
					.Include(c=>c.Subject)
					.Include(c=>c.Creator)
					.Include(c=>c.ContentCourses)
					.Select(c => new
					{
						c.CourseId,
						c.Code,
						SubjectName = c.Subject.Name,
						c.CourseName,
						c.Semester,
						c.Year,
						c.CreatorId,
						Creator = c.Creator.FirstName+" "+c.Creator.LastName,
						ContentCourses = c.ContentCourses.Select(ctc => new
						{
							ctc.ContentId,
							ctc.Title
						})
					})
					.FirstOrDefault(c => c.CourseId == courseId);
				if (course == null)
				{
					return NotFound();
				}
				return Ok(course);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


		[HttpPut("UpdateCourse")]
		public ActionResult Update(Course course)
		{
			try
			{
				_context.Courses.Update(course);
				_context.SaveChanges();
				return Ok("Success Register");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("DeleteCourse")]
		public ActionResult Delete(int courseId, int creatorId)
		{
			try
			{
				var course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId && c.CreatorId == creatorId);
				if (course == null)
				{
					return NotFound();
				}
				_context.Courses.Remove(course);
				_context.SaveChanges();
				return Ok("Success Delete");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
