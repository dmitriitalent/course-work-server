using Azure.Core;
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


        private string GetRole(string refreshToken)
        {
			JwtSecurityToken refreshTokenJwt = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);
			return refreshTokenJwt.Claims.LastOrDefault(c => c.Type == "Role").Value;
		}

		// Верифицирует токен пользователя при его наличии
		public bool IsAuthenticated(string refreshToken)
        {
/*            string refreshToken = null;
            Cookies.TryGetValue("RefreshToken", out refreshToken);*/
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

		public bool IsAdmin(string refreshToken)
        {
			if (!IsAuthenticated(refreshToken))
			{
				return false;
			}

			if (GetRole(refreshToken) == "Admin")
			{
				return true;
			}
			return false;
		}
	}
}
