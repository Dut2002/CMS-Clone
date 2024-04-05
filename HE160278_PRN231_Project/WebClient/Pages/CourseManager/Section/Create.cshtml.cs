using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Security.Claims;
using WebClient.Connection;

namespace WebClient.Pages.CourseManager.Section
{
	[Authorize(Roles = "Teacher")]
	public class CreateModel : PageModel
    {
		private readonly ICallApi _callApi;

		public CreateModel(ICallApi callApi)
		{
			_callApi = callApi;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public int CourseId { get; set; }


		public class InputModel
		{
			[Required]
			public int CourseId { get; set; }
			[Required]
			public string Title { get; set; }
			[Required]
			public int UserId {  get; set; }
		}

		[ViewData]
		public string ErrorMessage { get; set; }
		[ViewData]
		public string SuccessMessage { get; set; }

		public async Task<IActionResult> OnGetAsync(int? courseId)
		{
			if (!User.Identity.IsAuthenticated || courseId == null)
			{
				return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
			}
			CourseId = courseId.Value;
			var claimsPrincipal = HttpContext.User;
			var accountIdClaim = claimsPrincipal.FindFirstValue("AccountId");

			if (accountIdClaim == null)
			{
				return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
			}

			try
			{
				var (statusCode, content) = await _callApi.Get($"/Courses/GetCourseById?courseId={courseId}&creatorId={int.Parse(accountIdClaim)}");
				if (statusCode == HttpStatusCode.NotFound || statusCode == HttpStatusCode.Unauthorized)
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
							(HttpStatusCode statusCode, string content) = await _callApi.Post("/CourseContents/CreateContent", Input);
							if (statusCode == HttpStatusCode.OK)
							{
								return RedirectToPage("/CourseManager/Detail", new { courseId = Input.CourseId });
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
