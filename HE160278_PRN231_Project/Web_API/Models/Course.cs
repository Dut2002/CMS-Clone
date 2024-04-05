using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_API.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }

        [Required]
        public string Code { get; set; }

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

        [Required]
        public int CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        public User? Creator { get; set; }

        [ForeignKey(nameof(Code))]
        public Subject? Subject { get; set; }

        public ICollection<ContentCourse>? ContentCourses { get; set; } // Một Course có nhiều ContentCourse
        public ICollection<CourseManager>? CourseManagers { get; set; } // Một Course có nhiều CourseManager

    }

    public enum Semester
    {
        Spring = 1,
        Summer = 2,
        Fall = 3
    }
}
