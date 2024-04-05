// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using WebClient.Connection;

namespace WebClient.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class Register : PageModel
    {

        private readonly ICallApi _callApi;

        public Register(ICallApi callApi)
        {
            _callApi = callApi;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [ViewData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email format")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required(ErrorMessage = "Confirm password is required")]
            [Compare("Password", ErrorMessage = "Passwords do not match")]
            [DataType(DataType.Password)]
            public string Confirm { get; set; }

            [Required(ErrorMessage = "First name is required")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last name is required")]
            public string LastName { get; set; }
        }
        
        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    (HttpStatusCode statusCode, string content) = await _callApi.Post("/Users/Register", Input);
                    if (statusCode == HttpStatusCode.OK)
                    {
                        return RedirectToPage("/Account/Login");
                    };
                    if (statusCode != HttpStatusCode.BadRequest)
                    {
                        ErrorMessage = "Error Register";
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
