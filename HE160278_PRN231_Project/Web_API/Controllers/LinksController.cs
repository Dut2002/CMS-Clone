using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API.Models;
using Web_API.Models.Dto;

namespace Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LinksController : ControllerBase
	{
		private readonly ApplicationDBContext _context;
		public LinksController(ApplicationDBContext context)
		{
			_context = context;
		}

		[HttpPost("CreateLink")]
		public ActionResult Create(LinkDto linkDto)
		{
			try
			{
				var content = _context.ContentCourses.FirstOrDefault(cm => cm.ContentId == linkDto.ContentId);
				if (content == null)
				{
					return NotFound();
				}
				var manager = _context.CourseManagers.Where(m => m.CourseId == content.CourseId).Select(m => m.UserId).ToList();
				if (!manager.Contains(linkDto.UserId))
				{
					return Unauthorized();
				}
				var link = new Link
				{
					ContentId = linkDto.ContentId,
					Title = linkDto.Title,
					LinkAddress = linkDto.LinkAddress
				};
				_context.Links.Add(link);
				_context.SaveChanges();
				return Ok("Success Create");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
