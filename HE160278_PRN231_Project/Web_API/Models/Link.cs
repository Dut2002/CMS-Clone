using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web_API.Models
{
    public class Link
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LinkId { get; set; }

        [ForeignKey("ContentCourse")]
        public int ContentId { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string LinkAddress { get; set; }

        public ContentCourse ContentCourse { get; set; }
    }
}
