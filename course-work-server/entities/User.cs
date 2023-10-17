using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

namespace course_work_server.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Login { get; set; }
		[Required]
		public string Password { get; set; }
        public string Salt { get; set; }
		[Required]
		public string Role { get; set; }

        public RefreshToken RefreshToken { get; set; }

        public UserProfile Profile { get; set; }
    }

    public class UserProfile
    {
        public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Surname { get; set; }
		[Required]
		public string Email { get; set; }
        
        public int UserId { get; set; } 
        public User User { get; set; }
    }
}
