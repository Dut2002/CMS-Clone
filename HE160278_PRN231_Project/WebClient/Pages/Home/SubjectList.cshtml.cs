using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using WebClient.Connection;
using WebClient.Models;

namespace WebClient.Pages.Home
{
    public class SubjectListModel : PageModel
    {

        private readonly ICallApi _callApi;




        public SubjectListModel(ICallApi callApi)
        {
            _callApi = callApi;
        }

        public List<Subject> Subjects { get; set; } = new List<Subject>();

        [ViewData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                (HttpStatusCode statusCode, string content) = await _callApi.Get("/Subjects/GetSubjects");
                if (statusCode == HttpStatusCode.OK)
                {
                    Subjects = JsonConvert.DeserializeObject<List<Subject>>(content);
                }
                if (statusCode == HttpStatusCode.NotFound)
                {
                    ErrorMessage = "Not Found Any Subject";
                }
                if (statusCode == HttpStatusCode.BadRequest)
                {
                    ErrorMessage = "Error Subject List";
                    throw new Exception();
                }
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Server Error" +ex.Message;
                return RedirectToPage("/Error", ErrorMessage);
            }
        }
    }
}
