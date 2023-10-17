using System.ComponentModel.DataAnnotations;

namespace course_work_server.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
		[Required]
		public string Token { get; set; }
		[Required]
		public int UserId { get; set; }
        public User User { get; set; }
    }
}
