using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebClient.Models
{
    public class DownloadFile
    {
        public int DownloadId { get; set; }

        public int ContentId { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
