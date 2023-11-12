using course_work_server.Entities;
using course_work_server.Exceptions;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace course_work_server.Services
{
    public class AuthService
    {
        TokenService TokenService;

        public AuthService(TokenService TokenService)
        {
            this.TokenService = TokenService;
        }

        private JwtSecurityToken GetRefreshToken(IRequestCookieCollection Cookies)
        {
			string refreshTokenString = null;
			Cookies.TryGetValue("RefreshToken", out refreshTokenString);
			JwtSecurityToken refreshToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenString);
            return refreshToken;
        }

        private string GetRole(IRequestCookieCollection Cookies)
        {
            return GetRefreshToken(Cookies).Claims.LastOrDefault(c => c.Type == "Role").Value;
		}

		// Верифицирует токен пользователя при его наличии
		public bool IsAuthenticated(IRequestCookieCollection Cookies)
        {
            string refreshToken = null;
            Cookies.TryGetValue("RefreshToken", out refreshToken);
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

        public string GetClaimValue(string key, IRequestCookieCollection Cookies)
        {
            if(!IsAuthenticated(Cookies))
            {
                throw new UnauthorizedException<AuthService>();
            }
            
			return GetRefreshToken(Cookies).Claims.FirstOrDefault(c => c.Type == key).Value;
        }

		public bool IsAdmin(IRequestCookieCollection Cookies)
        {
			if (!IsAuthenticated(Cookies))
			{
				return false;
			}

			if (GetRole(Cookies) == "Admin")
			{
				return true;
			}
			return false;
		}
	}
}
