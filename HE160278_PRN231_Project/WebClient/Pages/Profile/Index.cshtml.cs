using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using WebClient.Connection;
using WebClient.Models.Dto;

namespace WebClient.Pages.Profile
{
	[Authorize(Roles = "Teacher")]
	public class IndexModel : PageModel
	{
		private readonly ICallApi _callApi;

		public IndexModel(ICallApi callApi)
		{
			_callApi = callApi;
		}


		[BindProperty]
		public InputModel Input { get; set; }


		public class InputModel
		{
			[Required]
			[EmailAddress]
			public string Email { get; set; }

			[Required]
			public string FirstName { get; set; }
			
			[Required]
			public string LastName { get; set; }
		}

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
						(HttpStatusCode statusCode, string content) = await _callApi.Get("/Users/Profile?userId=" + int.Parse(accountIdClaim)); ;
						if(statusCode == HttpStatusCode.OK)
						{
                            Input = JsonConvert.DeserializeObject<InputModel>(content);
							return Page();
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
							(HttpStatusCode statusCode, string content) = await _callApi.Put("/Users/UpdateProfile", new ProfileDto
							{
								UserId = int.Parse(accountIdClaim),
								Email = Input.Email,
								FirstName = Input.FirstName,
								LastName = Input.LastName,
							});
							if (statusCode == HttpStatusCode.OK)
							{

                                var claims = User.Claims;
                                var role = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role));

                                var accountClaims = new List<Claim>()
							{
								new Claim("AccountId", accountIdClaim),
								new Claim(ClaimTypes.Email, Input.Email),
								new Claim(ClaimTypes.Name, Input.FirstName + " " + Input.LastName),
								new Claim(ClaimTypes.Role, role.Value)
							};

								var accountIdentity = new ClaimsIdentity(accountClaims, "Account Identity");
								var accountPrincipal = new ClaimsPrincipal(new[] { accountIdentity });
								await HttpContext.SignInAsync(accountPrincipal);
								return RedirectToPage("/Index");
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
