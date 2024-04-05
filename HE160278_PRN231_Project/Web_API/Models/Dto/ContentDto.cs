using System.ComponentModel.DataAnnotations;

namespace Web_API.Models.Dto
{
	public class ContentDto
	{
		[Required]
		public int CourseId { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		public int UserId { get; set; }

		public int? ContentId { get; set; }
	}
}
