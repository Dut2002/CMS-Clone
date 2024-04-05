// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebClient.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public LogoutModel(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
                Response.Cookies.Delete("UserLoginCookie");
            }
            return RedirectToPage("/Index");
        }
    }
}
