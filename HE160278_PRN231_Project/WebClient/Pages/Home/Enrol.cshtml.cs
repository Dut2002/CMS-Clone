using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using WebClient.Connection;
using WebClient.Models;

namespace WebClient.Pages.Home
{
	[Authorize(Roles = "Teacher")]
	public class EnrolModel : PageModel
	{

		private readonly ICallApi _callApi;

		public EnrolModel(ICallApi callApi)
		{
			_callApi = callApi;
		}

		public Course Course { get; set; } = default!;
		public Models.CourseManager CourseManager { get; set; } = null;



		[ViewData]
		public string ErrorMessage { get; set; }

		[ViewData]
		public string SucccessMessage { get; set; }

		public async Task<IActionResult> OnGetAsync(int courseId)
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
						(HttpStatusCode statusCode, string content) = await _callApi.Get("/CourseManagers/GetCourseManager?courseId=" + courseId + "&userId=" + int.Parse(accountIdClaim));
						if (statusCode == HttpStatusCode.OK)
						{
							CourseManager = JsonConvert.DeserializeObject<Models.CourseManager>(content);
							if (CourseManager.IsStaff.HasValue && CourseManager.IsStaff.Value)
							{
								return RedirectToPage("/CourseManager/Detail", new { courseId });
							}
							(HttpStatusCode statusCode1, string content1) = await _callApi.Get("/Courses/GetCourse?courseId=" + courseId);
							if (statusCode1 == HttpStatusCode.OK)
							{
								Course = JsonConvert.DeserializeObject<Course>(content1);

								return Page();
							}
							return NotFound();
						}
						if (statusCode == HttpStatusCode.NotFound)
						{
							(HttpStatusCode statusCode1, string content1) = await _callApi.Get("/Courses/GetCourse?courseId=" + courseId);
							if (statusCode1 == HttpStatusCode.OK)
							{
								Course = JsonConvert.DeserializeObject<Course>(content1);

								return Page();
							}
							return NotFound();
						}
						return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });

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

		public async Task<IActionResult> OnPostAsync(int courseId)
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
						(HttpStatusCode statusCode, string content) = await _callApi.Post("/CourseManagers/CreateCourseManager?courseId=" + courseId + "&userId=" + int.Parse(accountIdClaim), new {});
						if (statusCode == HttpStatusCode.OK)
						{
							return RedirectToPage("/Home/Enrol" ,new {courseId});
						}
						ErrorMessage = "Enroll error";
						return RedirectToPage("/Error", ErrorMessage);
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
