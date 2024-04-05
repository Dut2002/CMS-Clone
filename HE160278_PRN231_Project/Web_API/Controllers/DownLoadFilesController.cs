using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_API.Models.Dto;
using Web_API.Models;
using Newtonsoft.Json;

namespace Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DownLoadFilesController : ControllerBase
	{
		private readonly ApplicationDBContext _context;
		private readonly IWebHostEnvironment _hostingEnvironment; // Đối tượng để truy cập thư mục wwwroot

		public DownLoadFilesController(ApplicationDBContext context, IWebHostEnvironment hostingEnvironment)
		{
			_context = context;
			_hostingEnvironment = hostingEnvironment;
		}

		[HttpPost("UpLoadFile")]
		public ActionResult Create()
		{
			try
			{
				var file = Request.Form.Files.FirstOrDefault(); // Lấy tệp tin từ yêu cầu
				var data = JsonConvert.DeserializeObject<DownloadFileDto>(Request.Form["data"]); // Lấy dữ liệu từ yêu cầu

				// Xử lý tệp tin và dữ liệu nhận được
				if (file == null || data == null)
				{
					return BadRequest("File or data is null.");
				}

				var content = _context.ContentCourses.FirstOrDefault(cm => cm.ContentId == data.ContentId);
				if (content == null)
				{
					return NotFound();
				}

				var manager = _context.CourseManagers.Where(m => m.CourseId == content.CourseId).Select(m => m.UserId).ToList();
				if (!manager.Contains(data.UserId))
				{
					return Unauthorized();
				}

				// Tạo đường dẫn cho file
				var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, content.CourseId.ToString(), content.ContentId.ToString(), file.FileName);

				// Kiểm tra xem thư mục cha có tồn tại không, nếu không thì tạo mới
				var directory = Path.GetDirectoryName(filePath);
				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}

				// Ghi file vào thư mục wwwroot
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					file.CopyTo(fileStream);
				}

				var fileRecord = new DownloadFile
				{
					ContentId = data.ContentId,
					Title = data.Title,
					LinkDownload = filePath
				};

				_context.DownloadFiles.Add(fileRecord);
				_context.SaveChanges();

				return Ok("Success Create");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


		[HttpGet("DownloadFile")]
		public IActionResult DownloadFile(int downloadId)
		{
			try
			{
				var fileRecord = _context.DownloadFiles.FirstOrDefault(df => df.DownloadId == downloadId);
				if (fileRecord == null)
				{
					return NotFound("File not found.");
				}

				var filePath = fileRecord.LinkDownload;

				// Kiểm tra xem tệp có tồn tại không
				if (!System.IO.File.Exists(filePath))
				{
					return NotFound("File not found.");
				}

				// Đọc dữ liệu từ tệp
				var fileBytes = System.IO.File.ReadAllBytes(filePath);
				var fileName = Path.GetFileName(filePath);

				// Trả về tệp như là một phản hồi HTTP và thêm phần mở rộng vào header
				Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"; filename*=UTF-8''{fileName}");
				return File(fileBytes, "application/octet-stream");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}




	
}