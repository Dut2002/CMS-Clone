using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API.Models;
using Web_API.Models.Dto;

namespace Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CourseContentsController : ControllerBase
	{
		private readonly ApplicationDBContext _context;
		public CourseContentsController(ApplicationDBContext context)
		{
			_context = context;
		}

		[HttpGet("GetContentById")]
		public ActionResult GetById(int contentId, int userId)
		{
			try
			{

				var content = _context.ContentCourses.FirstOrDefault(ctc => ctc.ContentId == contentId);
				if (content == null)
				{
					return NotFound();
				}
				var courseManager = _context.CourseManagers.Where(cm => cm.CourseId == content.CourseId && cm.UserId == userId && cm.IsStaff == true).ToList();
				if (!courseManager.Any())
				{
					return Unauthorized();
				}

				return Ok(new
				{
					content.ContentId,
					content.CourseId,
					content.Title,
					UserId = userId
				});
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("GetContent")]
		public ActionResult GetContent(int contentId, int userId)
		{
			try
			{

				var content = _context.ContentCourses
					.Include(ctc => ctc.Course.Creator)
					.Include(ctc => ctc.Course.Subject)
					.FirstOrDefault(ctc => ctc.ContentId == contentId);
				if (content == null)
				{
					return NotFound();
				}
				var courseManager = _context.CourseManagers.Where(cm => cm.CourseId == content.CourseId && cm.UserId == userId && cm.IsStaff == true).ToList();
				if (!courseManager.Any())
				{
					return Unauthorized();
				}

				return Ok(new
				{
					content.ContentId,
					Course = new {
						content.CourseId,
						content.Course.CourseName,
						content.Course.Code,
						SubjectName = content.Course.Subject.Name,
						content.Course.Semester,
						content.Course.Year,
						CreatorId = content.Course.Creator.UserId,
						Creator = content.Course.Creator.FirstName + " "+ content.Course.Creator.LastName,
					},
					content.Title,
					Documents = _context.Documents.Where(d=>d.ContentId == content.ContentId).Select(d => new
					{
						d.DocumentId,
						d.Title,
						d.Content
					}),
					DownloadFiles = _context.DownloadFiles.Where(d => d.ContentId == content.ContentId).Select(d => new
					{
						d.DownloadId,
						d.Title,
						d.LinkDownload
					}),
					Assignments = _context.Assignments.Where(d => d.ContentId == content.ContentId).Select(d => new
					{
						d.AssignmentId,
						d.Title,
						d.Description,
						d.Deadline
					}),
					Links = _context.Links.Where(d => d.ContentId == content.ContentId).Select(d => new
					{
						d.LinkId,
						d.Title,
						d.LinkAddress
					})


				});
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("CreateContent")]
		public ActionResult Create(ContentDto contentDto)
		{
			try
			{
				var courseManager = _context.CourseManagers.Where(cm => cm.CourseId == contentDto.CourseId && cm.UserId == contentDto.UserId && cm.IsStaff == true).ToList();
				if (!courseManager.Any())
				{
					return Unauthorized();
				}
				var content = new ContentCourse
				{
					CourseId = contentDto.CourseId,
					Title = contentDto.Title,
				};
				_context.ContentCourses.Add(content);
				_context.SaveChanges();
				return Ok("Success Register");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


		[HttpPut("UpdateContent")]
		public ActionResult Update(ContentDto contentDto)
		{
			try
			{
				var content = _context.ContentCourses.FirstOrDefault(ctc => ctc.ContentId == contentDto.ContentId);
				if (content == null)
				{
					return NotFound();
				}
				var courseManager = _context.CourseManagers.Where(cm => cm.CourseId == content.CourseId && cm.UserId == contentDto.UserId && cm.IsStaff == true).ToList();
				if (!courseManager.Any())
				{
					return Unauthorized();
				}
				content.Title = contentDto.Title; 
				_context.ContentCourses.Update(content);
				_context.SaveChanges();
				return Ok("Success Update");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("DeleteContent")]
		public ActionResult Delete(int contentId, int userId)
		{
			try
			{
				var content = _context.ContentCourses.FirstOrDefault(ctc => ctc.ContentId == contentId);
				if (content == null)
				{
					return NotFound();
				}
				var courseManager = _context.CourseManagers.Where(cm => cm.CourseId == content.CourseId && cm.UserId == userId && cm.IsStaff == true).ToList();
				if (!courseManager.Any())
				{
					return Unauthorized();
				}
				_context.ContentCourses.Remove(content);
				_context.SaveChanges();
				return Ok("Success Remove");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
