namespace WebClient.Models.Dto
{
	public class LoginDto
	{
        public int UserId { get; set; }
        public string  Email { get; set; }
		public string Username { get; set; }
		public string Role { get; set; }
        public string Token { get; set; }
    }
}
