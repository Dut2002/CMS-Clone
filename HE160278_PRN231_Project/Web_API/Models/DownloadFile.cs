using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web_API.Models
{
    public class DownloadFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DownloadId { get; set; }

        [ForeignKey("ContentCourse")]
        public int ContentId { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string LinkDownload { get; set; }

        public ContentCourse ContentCourse { get; set; }
    }
}
