using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web_API.Models.Dto
{
	public class DownloadFileDto
	{
		public int? DownloadId { get; set; }
		[Required]
		public int ContentId { get; set; }

		[Required]
		public string Title { get; set; }

		public int UserId { get; set; }
	}
}
