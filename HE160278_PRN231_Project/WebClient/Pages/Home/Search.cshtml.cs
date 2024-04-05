using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using WebClient.Connection;
using WebClient.Models;

namespace WebClient.Pages.Home
{
    public class SearchModel : PageModel
    {
		private readonly ICallApi _callApi;

		public SearchModel(ICallApi callApi)
		{
			_callApi = callApi;
		}

		public string? Search { get; set; }
		public string? Code { get; set; }

		public List<Course> Courses { get; set; } = new List<Course>();

		public SelectList Subjects { get; set; } = null;


		[ViewData]
		public string ErrorMessage { get; set; }

		public async Task<IActionResult> OnGetAsync(string? code, string? search)
		{
			try
			{
				(HttpStatusCode statusCode, string content) = await _callApi.Get("/Courses/GetCourses?code="+code+"&search="+search);
				(HttpStatusCode statusCode1, string content1) = await _callApi.Get("/Subjects/GetSubjects");
				if(statusCode1 != HttpStatusCode.BadRequest)
				{
					var subjects = JsonConvert.DeserializeObject<List<Subject>>(content1);
					Subjects = new SelectList(subjects, "Code", "Name");
				}
				if (statusCode == HttpStatusCode.OK )
				{
					Courses = JsonConvert.DeserializeObject<List<Course>>(content);
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
}
