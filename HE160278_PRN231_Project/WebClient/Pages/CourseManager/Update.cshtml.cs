using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using WebClient.Connection;
using WebClient.Models;

namespace WebClient.Pages.CourseManager
{
    public class UpdateModel : PageModel
    {
		private readonly ICallApi _callApi;

		public UpdateModel(ICallApi callApi)
		{
			_callApi = callApi;
		}

		[BindProperty]
		public InputModel Input { get; set; }



		public class InputModel
		{
			[Required]
			public int CourseId { get; set; }

			[Required]
			public string Code { get; set; }

			[Required]
			public string CourseName { get; set; }

			[Required]
			public Semester Semester { get; set; }

			[Required]
			[Range(2024, 2030, ErrorMessage = "Year must be between 2024 and 2030")]
			public int Year { get; set; }

			[Required]
			public int CreatorId { get; set; }
		}

		[ViewData]
		public SelectList Subjects { get; set; } = null;

		[ViewData]
		public string ErrorMessage { get; set; }

		public async Task<IActionResult> OnGetAsync(int? courseId)
		{
			if (!User.Identity.IsAuthenticated || courseId == null)
			{
				return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
			}

			var claimsPrincipal = HttpContext.User;
			var accountIdClaim = claimsPrincipal.FindFirstValue("AccountId");

			if (accountIdClaim == null)
			{
				return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
			}

			try
			{
				var (statusCode, content) = await _callApi.Get("/Subjects/GetSubjects");
				if (statusCode == HttpStatusCode.OK)
				{
					var subjects = JsonConvert.DeserializeObject<List<Subject>>(content);
					Subjects = new SelectList(subjects, "Code", "Name");
				}
				else if (statusCode == HttpStatusCode.NotFound || statusCode == HttpStatusCode.Unauthorized)
				{
					ErrorMessage = "Not Found Subject";
				}
				else if (statusCode == HttpStatusCode.BadRequest)
				{
					throw new Exception("Update Course Error");
				}

				var (statusCode1, content1) = await _callApi.Get($"/Courses/GetCourseById?courseId={courseId}&creatorId={int.Parse(accountIdClaim)}");
				if (statusCode1 == HttpStatusCode.OK)
				{
					Input = JsonConvert.DeserializeObject<InputModel>(content1);
				}
				else if (statusCode == HttpStatusCode.NotFound || statusCode == HttpStatusCode.Unauthorized)
				{
					return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
				}
				else if (statusCode == HttpStatusCode.BadRequest)
				{
					throw new Exception("Profile Error");
				}
				return Page();
			}
			catch (Exception ex)
			{
				ErrorMessage = "Server Error\n" + ex.Message;
				return RedirectToPage("/Error", new { message = ErrorMessage });
			}
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (User.Identity.IsAuthenticated)
			{
				var claimsPrincipal = HttpContext.User;

				// Tìm claim có loại là "AccountId"
				var accountIdClaim = claimsPrincipal.FindFirstValue("AccountId");

				if (accountIdClaim != null)
				{
					if (ModelState.IsValid)
					{
						try
						{
							(HttpStatusCode statusCode, string content) = await _callApi.Put("/Courses/UpdateCourse", new
							{
								Input.CourseId,
								Input.Code,
								Input.CourseName,
								Input.Semester,
								Input.Year,
								Input.CreatorId
							});
							if (statusCode == HttpStatusCode.OK)
							{
								return RedirectToPage("/CourseManager/Index");
							}
							if (statusCode == HttpStatusCode.Unauthorized)
							{
								return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
							}
							if (statusCode == HttpStatusCode.BadRequest)
							{
								throw new Exception("Profile Error");
							}
						}
						catch (Exception ex)
						{
							ErrorMessage = "Server Error\n" + ex.Message;
							return RedirectToPage("/Error", ErrorMessage);
						}
					}
					return Page();
				}
			}
			return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
		}
	}
}
