using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API.Models;
using Web_API.Models.Dto;

namespace Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DocumentsController : ControllerBase
	{
		private readonly ApplicationDBContext _context;
		public DocumentsController(ApplicationDBContext context)
		{
			_context = context;
		}

		[HttpPost("CreateDocument")]
		public ActionResult Create(DocumentDto documentDto)
		{
			try
			{
				var content = _context.ContentCourses.FirstOrDefault(cm => cm.ContentId == documentDto.ContentId);
				if (content == null)
				{
					return NotFound();
				}
				var manager = _context.CourseManagers.Where(m => m.CourseId == content.CourseId).Select(m => m.UserId).ToList();
				if(!manager.Contains(documentDto.UserId))
				{
					return Unauthorized();
				}
				var document = new Document
				{
					ContentId = documentDto.ContentId,
					Title = documentDto.Title,
					Content = documentDto.Content
				};
				_context.Documents.Add(document);
				_context.SaveChanges();
				return Ok("Success Register");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
