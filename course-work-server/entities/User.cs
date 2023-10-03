using System.Security;

namespace course_work_server.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        public string Salt { get; set; }

        public RefreshToken RefreshToken { get; set; }
    }
}
