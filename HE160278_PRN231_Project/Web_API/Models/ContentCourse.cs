using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web_API.Models
{
    public class ContentCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContentId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string Title { get; set; }

        // Một ContentCourse có thể thuộc về một trong các loại sau: Document, DownloadFile, Assignment, Quiz
        public ICollection<Document> Documents { get; set; }
        public ICollection<DownloadFile> DownloadFiles { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Link> Links { get; set; }
    }
}