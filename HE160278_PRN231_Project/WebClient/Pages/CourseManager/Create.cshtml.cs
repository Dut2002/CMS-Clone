using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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



		public class InputModel
		{
			[Required(ErrorMessage = "Code is required")]
			public string Code { get; set; }

			[Required(ErrorMessage = "Course name is required")]
			public string CourseName { get; set; }

			[Required(ErrorMessage = "Semester is required")]
			public Semester Semester { get; set; }

			[Required(ErrorMessage = "Year is required")]
			[Range(2024, 2030, ErrorMessage = "Year must be between 2024 and 2030")]
			public int Year { get; set; }

			[Required(ErrorMessage = "Start date is required")]
			public DateTime StartDate { get; set; }

			[Required(ErrorMessage = "End date is required")]
			[CustomValidation(typeof(InputModel), "ValidateEndDate")]
			public DateTime Enđate { get; set; }

			[Required(ErrorMessage = "Creator ID is required")]
			public int CreatorId { get; set; }

			public static ValidationResult ValidateEndDate(DateTime endDate, ValidationContext context)
			{
				var inputModel = context.ObjectInstance as InputModel;

				if (endDate <= inputModel.StartDate)
				{
					return new ValidationResult("End Date must be greater than Start Date.");
				}
				return ValidationResult.Success;
			}
		}

		public SelectList Subjects { get; set; } = null;

        [ViewData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
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
                        (HttpStatusCode statusCode, string content) = await _callApi.Get("/Subjects/GetSubjects");
                        if (statusCode == HttpStatusCode.OK)
                        {
                            var subjects = JsonConvert.DeserializeObject<List<Subject>>(content);
                            Subjects = new SelectList(subjects, "Code", "Name");
                        }

                        if (statusCode == HttpStatusCode.NotFound)
                        {
                            ErrorMessage = "Not Found Subject";
                        }
                        if (statusCode == HttpStatusCode.BadRequest)
                        {
                            throw new Exception("Profile Error");
                        }
                        return Page();

                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = "Server Error\n" + ex.Message;
                        return RedirectToPage("/Error", ErrorMessage);
                    }

                }
            }
            return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
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
                            (HttpStatusCode statusCode, string content) = await _callApi.Post("/Courses/CreateCourse", new
                            {
                                Input.Code,
                                Input.CourseName,
                                Input.Semester,
                                Input.Year,
                                Input.CreatorId,
                                Input.StartDate,
                                Input.Enđate,
								courseManagers = new[] { new { userId = Input.CreatorId, isStaff = true } }
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
					(HttpStatusCode statusCode1, string content1) = await _callApi.Get("/Subjects/GetSubjects");
					if (statusCode1 == HttpStatusCode.OK)
					{
						var subjects = JsonConvert.DeserializeObject<List<Subject>>(content1);
						Subjects = new SelectList(subjects, "Code", "Name");
					}
					return Page();
                }
            }
            return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
        }
    }
}
