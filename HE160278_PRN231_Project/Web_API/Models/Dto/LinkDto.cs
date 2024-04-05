using System.ComponentModel.DataAnnotations;

namespace Web_API.Models.Dto
{
	public class LinkDto
	{
		public int? LinkId { get; set; }
		[Required]
		public int ContentId { get; set; }

		[Required]
		public string Title { get; set; }
		[Required]
		public string LinkAddress { get; set; }
		[Required]
		public int UserId { get; set; }
	}
}
