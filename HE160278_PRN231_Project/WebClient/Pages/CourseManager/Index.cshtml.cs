using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using WebClient.Connection;
using WebClient.Models;

namespace WebClient.Pages.CourseManager
{
	[Authorize(Roles ="Teacher")]
    public class IndexModel : PageModel
    {
		private readonly ICallApi _callApi;

		public IndexModel(ICallApi callApi)
		{
			_callApi = callApi;
		}

		public string? Search { get; set; }
		public string? Code { get; set; }

		public List<Course> Courses { get; set; } = new List<Course>();

		public SelectList Subjects { get; set; } = null;


		[ViewData]
		public string ErrorMessage { get; set; }

		[ViewData]
		public string SucccessMessage { get; set; }

		public async Task<IActionResult> OnGetAsync(string? code, string? search)
		{
			if (User.Identity.IsAuthenticated)
			{
				var claimsPrincipal = HttpContext.User;

				// Tìm claim có loại là "AccountId"
				var accountIdClaim = claimsPrincipal.FindFirstValue("AccountId");

				if (accountIdClaim != null)
				{
					try
					{
						(HttpStatusCode statusCode, string content) = await _callApi.Get("/Courses/GetCourses?code=" + code + "&search=" + search + "&userId=" +int.Parse(accountIdClaim));
						(HttpStatusCode statusCode1, string content1) = await _callApi.Get("/Subjects/GetSubjects");
						if (statusCode1 != HttpStatusCode.BadRequest)
						{
							var subjects = JsonConvert.DeserializeObject<List<Subject>>(content1);
							Subjects = new SelectList(subjects, "Code", "Name");
						}
						if (statusCode == HttpStatusCode.OK)
						{
							Courses = JsonConvert.DeserializeObject<List<Course>>(content);
						}
						if (statusCode == HttpStatusCode.Unauthorized)
						{
							return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
						}
						if (statusCode == HttpStatusCode.NotFound)
						{
							ErrorMessage = "Not Found Any Course";
						}
						if (statusCode == HttpStatusCode.BadRequest)
						{
							throw new Exception("Error Search");
						}
						return Page();
					}
					catch (Exception ex)
					{
						ErrorMessage = "Server Error" + ex.Message;
						return RedirectToPage("/Error", ErrorMessage);
					}
				}
			}
			return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
		}

		[HttpPost]
		public async Task<IActionResult> OnPostAsync(int? courseId)
		{
			if (User.Identity.IsAuthenticated && courseId!=null)
			{
				var claimsPrincipal = HttpContext.User;

				// Tìm claim có loại là "AccountId"
				var accountIdClaim = claimsPrincipal.FindFirstValue("AccountId");

				if (accountIdClaim != null)
				{
					try
					{
						(HttpStatusCode statusCode, string content) = await _callApi.Delete("/Courses/DeleteCourse?courseId=" + courseId + "&creatorId=" + int.Parse(accountIdClaim));
						if (statusCode == HttpStatusCode.OK)
						{
							SucccessMessage = "Xóa khóa học thành công";
							return RedirectToPage("/CourseManager/Index");
						}
						if (statusCode == HttpStatusCode.Unauthorized || statusCode == HttpStatusCode.NotFound)
						{
							return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
						}
						if (statusCode == HttpStatusCode.BadRequest)
						{
							throw new Exception("Error Delete");
						}
						return Page();
					}
					catch (Exception ex)
					{
						ErrorMessage = "Server Error" + ex.Message;
						return RedirectToPage("/Error", ErrorMessage);
					}
				}
			}
			return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
		}
	}
}
