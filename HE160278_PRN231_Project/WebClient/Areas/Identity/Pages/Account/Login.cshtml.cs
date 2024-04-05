// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebClient.Connection;
using System.Net;
using NuGet.Protocol.Plugins;
using Newtonsoft.Json;
using WebClient.Models.Dto;
using System.Net.Http;

namespace WebClient.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
	public class LoginModel : PageModel
	{
		private readonly ICallApi _callApi;
		private readonly HttpClient _httpClient;

		public LoginModel(ICallApi callApi, HttpClient httpClient)
		{
			_callApi = callApi;
			_httpClient = httpClient;
		}

		[BindProperty]
        public InputModel Input { get; set; }

        [ViewData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
                Response.Cookies.Delete("UserLoginCookie");

            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
			if (User.Identity.IsAuthenticated)
			{
				await HttpContext.SignOutAsync();
                Response.Cookies.Delete("UserLoginCookie");
            }
            if (ModelState.IsValid)
			{
				try
				{
                    (HttpStatusCode statusCode, string content) = await _callApi.Get("/Users/Login?email=" + Input.Email + "&password=" + Input.Password);
                    if (statusCode == HttpStatusCode.OK)
                    {
                        LoginDto login = JsonConvert.DeserializeObject<LoginDto>(content);

                        // Lưu token vào cookie
                        // Thiết lập token vào tiêu đề Authorization của HttpClient
                        string token = login.Token;

                        var cookieOptions = new CookieOptions
                        {
                            Expires = DateTimeOffset.UtcNow.AddDays(1), // Thiết lập thời gian hết hạn của cookie
                            HttpOnly = true, // Chỉ cho phép truy cập cookie qua HTTP
                            Secure = true, // Đảm bảo cookie chỉ được gửi qua kênh an toàn (HTTPS)
                            SameSite = SameSiteMode.Strict // Thiết lập SameSite để ngăn chặn các cuộc tấn công Cross-Site Request Forgery (CSRF)
                        };

                        // Lưu token vào cookie
                        Response.Cookies.Append("UserLoginCookie", token, cookieOptions);

                        //Kiểm tra quyền và lưu account vào session

                        var accountClaims = new List<Claim>()
                    {
                        new Claim("AccountId", login.UserId.ToString()),
                        new Claim(ClaimTypes.Email, login.Email),
                        new Claim(ClaimTypes.Name, login.Username),
                        new Claim(ClaimTypes.Role, login.Role)

                    };

                        var accountIdentity = new ClaimsIdentity(accountClaims, "Account Identity");
                        var accountPrincipal = new ClaimsPrincipal(new[] { accountIdentity });
                        await HttpContext.SignInAsync(accountPrincipal);

                        return RedirectToPage("/Index");
                    }
                    if (statusCode == HttpStatusCode.Unauthorized)
                    {
                        ErrorMessage = "Wrong username or password";
                    }
                    if (statusCode == HttpStatusCode.BadRequest)
                    {
                        ErrorMessage = "Error Login";
                    }
                }
                catch (Exception ex)
				{
                    ErrorMessage = "Server Error\n" + ex.Message;
				}
			}
			return Page();
		}
    }
}
