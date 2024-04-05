using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using WebClient.Connection;

namespace WebClient.Pages.CourseManager.DownloadFileManager
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

		public int ContentId { get; set; }



		public class InputModel
		{
			public int DownloadId { get; set; }

			public int ContentId { get; set; }

			[Required]
			public string Title { get; set; }

			[Required]
			public int UserId { get; set; }



		}

		[BindProperty]
		[Required(ErrorMessage = "Please select a file.")]
		[DataType(DataType.Upload)]
		[AllowedExtensions(new string[] { ".txt", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".zip" })]
		[MaxFileSize(100 * 1024 * 1024)] // 100MB
		public IFormFile File { get; set; }



		public class AllowedExtensionsAttribute : ValidationAttribute
		{
			private readonly string[] _extensions;

			public AllowedExtensionsAttribute(string[] extensions)
			{
				_extensions = extensions;
			}

			protected override ValidationResult IsValid(
				object value, ValidationContext validationContext)
			{
				if (value is IFormFile file)
				{
					var extension = Path.GetExtension(file.FileName);

					if (!_extensions.Contains(extension.ToLower()))
					{
						return new ValidationResult(GetErrorMessage());
					}
				}

				return ValidationResult.Success;
			}

			public string GetErrorMessage()
			{
				return $"This file extension is not allowed!";
			}
		}

		public class MaxFileSizeAttribute : ValidationAttribute
		{
			private readonly long _maxFileSize;

			public MaxFileSizeAttribute(long maxFileSize)
			{
				_maxFileSize = maxFileSize;
			}

			protected override ValidationResult IsValid(
				object value, ValidationContext validationContext)
			{
				if (value is IFormFile file)
				{
					if (file.Length > _maxFileSize)
					{
						return new ValidationResult(GetErrorMessage());
					}
				}

				return ValidationResult.Success;
			}

			public string GetErrorMessage()
			{
				return $"File size cannot exceed {_maxFileSize / 1024 / 1024} MB!";
			}
		}



		[ViewData]
		public string ErrorMessage { get; set; }
		[ViewData]
		public string SuccessMessage { get; set; }

		public async Task<IActionResult> OnGetAsync(int? contentId)
		{
			if (!User.Identity.IsAuthenticated || contentId == null)
			{
				return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
			}
			ContentId = contentId.Value;
			var claimsPrincipal = HttpContext.User;
			var accountIdClaim = claimsPrincipal.FindFirstValue("AccountId");

			if (accountIdClaim == null)
			{
				return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
			}

			try
			{
				var (statusCode, content) = await _callApi.Get($"/CourseContents/GetContentById?contentId={contentId}&userId={int.Parse(accountIdClaim)}");
				if (statusCode == HttpStatusCode.OK)
				{
					return Page();
				}
				else if (statusCode == HttpStatusCode.BadRequest)
				{
					throw new Exception("Profile Error");
				}
				return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });

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
				var accountIdClaim = claimsPrincipal.FindFirstValue("AccountId");

				if (accountIdClaim != null)
				{
					if (ModelState.IsValid)
					{
						try
						{
							// Gọi phương thức PostFile với các tham số cần thiết
							var (statusCode, responseContent) = await _callApi.PostFile("/DownLoadFiles/UpLoadFile", File.OpenReadStream(), File.FileName, Input);

							// Xử lý kết quả trả về
							if (statusCode == HttpStatusCode.OK)
							{
								return RedirectToPage("/CourseManager/Section/Index", new { contentId = Input.ContentId });
							}
							if (statusCode == HttpStatusCode.Unauthorized)
							{
								return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
							}
							throw new Exception("Profile Error");
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
