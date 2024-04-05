using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_API.Models;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public SubjectsController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet("GetSubjects")]
        public ActionResult GetSubjects()
        {
            try
            {
                var subjects = _context.Subjects.ToList();
                if (!subjects.Any())
                {
                    return NotFound();
                }
                else
                {
                    return Ok(subjects);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
