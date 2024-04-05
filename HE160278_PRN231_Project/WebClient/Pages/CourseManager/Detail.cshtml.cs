﻿using Microsoft.AspNetCore.Authorization;
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
    public class DetailModel : PageModel
    {
		private readonly ICallApi _callApi;

		public DetailModel(ICallApi callApi)
		{
			_callApi = callApi;
		}

		[ViewData]
		public string ErrorMessage { get; set; }

		[ViewData]
		public string SucccessMessage { get; set; }

		public Course Course { get; set; } = default!;


		public async Task<IActionResult> OnGetAsync(int? courseId)
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

						(HttpStatusCode statusCode, string content) = await _callApi.Get("/Courses/GetCourseWithContent?courseId=" + courseId + "&userId=" + int.Parse(accountIdClaim));
						if (statusCode == HttpStatusCode.OK)
						{
							Course = JsonConvert.DeserializeObject<Course>(content);
							return Page();
						}
						if (statusCode == HttpStatusCode.BadRequest)
						{
							throw new Exception("Error Search");
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

		[HttpPost]
		public async Task<IActionResult> OnPostAsync(int? contentId, int? courseId)
		{
			if (User.Identity.IsAuthenticated && contentId != null && courseId != null)
			{
				var claimsPrincipal = HttpContext.User;

				// Tìm claim có loại là "AccountId"
				var accountIdClaim = claimsPrincipal.FindFirstValue("AccountId");

				if (accountIdClaim != null)
				{
					try
					{
						(HttpStatusCode statusCode, string content) = await _callApi.Delete("/CourseContents/DeleteContent?contentId=" + contentId + "&userId=" + int.Parse(accountIdClaim));
						if (statusCode == HttpStatusCode.OK)
						{
							SucccessMessage = "Xóa nội dung thành công";
							return RedirectToPage("/CourseManager/Detail", new { courseId = courseId.Value });
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
