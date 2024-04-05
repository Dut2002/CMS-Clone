using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using WebClient.Connection;
using WebClient.Models;

namespace WebClient.Pages.CourseManagement
{
    [Authorize(Roles ="Teacher")]
    public class IndexModel : PageModel
    {
		private readonly ICallApi _callApi;

		public IndexModel(ICallApi callApi)
		{
			_callApi = callApi;
		}
		[ViewData]
		public string ErrorMessage { get; set; }

		[ViewData]
		public string SucccessMessage { get; set; }
		public Course Course { get; set; }
        public async Task<IActionResult> OnGet(int courseId)
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

						(HttpStatusCode statusCode, string content) = await _callApi.Get("/Courses/GetCourseById?courseId=" + courseId + "&creatorId=" + int.Parse(accountIdClaim));
						if (statusCode == HttpStatusCode.OK)
						{
							Course = JsonConvert.DeserializeObject<Course>(content);
							return Page();
						}
						if (statusCode == HttpStatusCode.NotFound)
						{
							return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
						}
						throw new Exception("Error Search");

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
