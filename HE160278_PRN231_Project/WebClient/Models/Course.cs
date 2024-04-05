using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebClient.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }

        [Required]
        public string Code { get; set; }
        [Required]
        public string SubjectName { get; set; }

        [Required]
        public string CourseName { get; set; }

		[Required]
		public DateTime StartDate { get; set; }

        [Required]
        public DateTime Enđate { get; set; }

        [Required]
        public Semester Semester { get; set; }

        [Required]
        public int Year { get; set; }

        public int CreatorId { get; set; }

        public string Creator { get; set; }

        public ICollection<ContentCourse> ContentCourses { get; set; } // Một Course có nhiều ContentCourse
        public ICollection<User> Managers { get; set; } // Một Course có nhiều CourseManager

    }

    public enum Semester
    {
        Spring = 1,
        Summer = 2,
        Fall = 3
    }
}
