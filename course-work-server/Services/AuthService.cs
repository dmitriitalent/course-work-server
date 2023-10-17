using course_work_server.Entities;
using course_work_server.Exceptions;
using System.IdentityModel.Tokens.Jwt;

namespace course_work_server.Services
{
    public class AuthService
    {
        TokenService TokenService;
        HttpRequest Request;

        public AuthService(HttpRequest Request, TokenService TokenService)
        {
            this.Request = Request;
            this.TokenService = TokenService;
        }

        private JwtSecurityToken GetRefreshToken()
        {
			string refreshTokenString = null;
			Request.Cookies.TryGetValue("RefreshToken", out refreshTokenString);
			JwtSecurityToken refreshToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenString);
			return refreshToken;
		}

        private string GetRole()
        {
            return GetRefreshToken().Claims.LastOrDefault(c => c.Type == "Role").Value;
		}

		// Верифицирует токен пользователя при его наличии
		public bool IsAuthenticated()
        {
            string refreshToken = null;
            Request.Cookies.TryGetValue("RefreshToken", out refreshToken);
            if (refreshToken == null) 
            { 
                return false; 
            }
            try
            {
                TokenService.VerifyToken(refreshToken);
                return true;
            }
            catch (UnauthorizedException<TokenService> ex)
            {
                return false;
            }
		}

        public string GetClaimValue(string key)
        {
            if(!IsAuthenticated())
            {
                throw new UnauthorizedException<AuthService>();
            }
            
			return GetRefreshToken().Claims.FirstOrDefault(c => c.Type == key).Value;
        }

		public bool IsAdmin()
		{
			if (!IsAuthenticated())
			{
				return false;
			}

			if (GetRole() == "Admin")
			{
				return true;
			}
			return false;
		}
	}
}
