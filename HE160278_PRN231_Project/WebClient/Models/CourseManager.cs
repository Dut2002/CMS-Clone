using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebClient.Models;

namespace WebClient.Models
{
    public class CourseManager
    {
        public int CourseManagerId { get; set; }

        public int CourseId { get; set; }
        public string Course { get; set; }

        public int UserId { get; set; }
        public string User { get; set; }

        public bool? IsStaff { get; set; }
    }
}
