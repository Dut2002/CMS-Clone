using System.ComponentModel.DataAnnotations;

namespace Web_API.Models
{
    public class Subject
    {
        [Key]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<Course> Courses { get; set; } // Một Course có nhiều ContentCourse

    }
}
