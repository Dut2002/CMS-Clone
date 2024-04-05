using System.ComponentModel.DataAnnotations;

namespace Web_API.Models.Dto
{
	public class DocumentDto
	{
		public int? DocumentId { get; set; }
		[Required]
		public int ContentId { get; set; }

		[Required]
		public string Title { get; set; }
		[Required]
		public string Content { get; set; }
		[Required]
		public int UserId { get; set; }
	}
}
