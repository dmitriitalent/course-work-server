using course_work_server.Dto;
using course_work_server.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace course_work_server.Services
{
	public class LoginService
	{
		DataContext db;
		TokenService TokenService;
		private HashPasswordService HashPasswordService;

        public LoginService(DataContext db) 
		{
			this.db = db;
            this.TokenService = new TokenService(db);
            this.HashPasswordService = new HashPasswordService();

		}

        public string? CheckUser(LoginDTO loginDTO)
        {
	        IQueryable<User> users = db.Users.Where(user => user.Login == loginDTO.Login);
	        var passwordCompared = false;
	        foreach (var user in users)
	        {
		        passwordCompared = HashPasswordService.Compare(user.Password, loginDTO.Password);
	        }
			if (!passwordCompared) 
			{
				return "Неверный логин или пароль";
			}
			return null;

			
		}

	}
}
