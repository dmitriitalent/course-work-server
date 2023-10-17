using course_work_server.Entities;
using course_work_server.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace course_work_server.Services
{
	public class TokenService
	{
		DataContext db;
		LoggerService LoggerService;
		public TokenService(DataContext db)
		{
			this.db = db;
			this.LoggerService = new LoggerService();
		}

		public IList<JwtSecurityToken> GenerateTokens(User user)
		{
			IList<Claim> claims = new List<Claim>() {
				new Claim("Login", user.Login),
				new Claim("Role", user.Role),
				new Claim("Id", user.Id.ToString())
			};

			JwtSecurityToken accessToken = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
				signingCredentials: new SigningCredentials(TokenSettings.GetSymmetricSecurityAccessKey(), SecurityAlgorithms.HmacSha256)
			);
			JwtSecurityToken refreshToken = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.UtcNow.Add(TimeSpan.FromDays(30)),
				signingCredentials: new SigningCredentials(TokenSettings.GetSymmetricSecurityRefreshKey(), SecurityAlgorithms.HmacSha256)
			);

			return new List<JwtSecurityToken>() { accessToken, refreshToken };
		}

		public void SaveToken(string NewToken, User user)
		{
			RefreshToken token = db.RefreshTokens.FirstOrDefault(token => token.UserId == user.Id);
			if (token != null)
			{
				try
				{
					token.Token = NewToken;
					db.SaveChanges();
				}
				catch (Exception ex)
				{
					LoggerService.LogError(ex);
				}
				return;
			}
			else
			{
				RefreshToken RefreshToken = new RefreshToken()
				{
					Token = NewToken,
					User = user,
					UserId = user.Id
				};
				try {
					db.RefreshTokens.Add(RefreshToken);
					db.SaveChanges();
				}
				catch (Exception ex)
				{
					LoggerService.LogError(ex);
				}
				return;
			}
		}

		public void RemoveToken(string refreshToken)
		{
			try
			{
				db.RefreshTokens.Remove(
					db.RefreshTokens.FirstOrDefault(token => token.Token == refreshToken)
				);
				db.SaveChanges();
			}
			catch (Exception ex)
			{
				LoggerService.LogError(ex);
			}
		}

		public bool VerifyToken(string refreshToken)
		{
			JwtSecurityToken token = new JwtSecurityTokenHandler().ReadToken(refreshToken) as JwtSecurityToken;
			if (token.ValidTo < DateTime.UtcNow)
			{
				throw new UnauthorizedException<TokenService>("У токена закончилось время жизни");
			}

			var validToken = new JwtSecurityToken(
				claims: token.Claims,
				expires: token.ValidTo,
				signingCredentials: new SigningCredentials(TokenSettings.GetSymmetricSecurityRefreshKey(), SecurityAlgorithms.HmacSha256)
			);

			if (new JwtSecurityTokenHandler().WriteToken(validToken) != refreshToken)
			{
				throw new UnauthorizedException<TokenService>("Подпись токена не совпадает");
			}
			return true;
		}

		public bool ExistDbRefreshToken(string refreshToken)
		{
			if (db.RefreshTokens.FirstOrDefault(token => token.Token == refreshToken) == null) 
			{ return false; }
			return true;
		}

        public User GetUserByToken(string token)
        {
			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(token);
			string userId = jwtToken.Claims.FirstOrDefault(cl => cl.Type == "Id").Value;

            User user = db.Users.Include(u => u.Profile).FirstOrDefault(user => 
				user.Id.ToString() == userId
            );
			return user;
        }
    }

	public static class TokenSettings
	{
		public const string ISSUER = "MyAuthServer"; // издатель токена
		public const string AUDIENCE = "MyAuthClient"; // потребитель токена
		const string ACCESS_KEY = "ACCESS_TOKEN_KEY!git!google!pepsi!starbucks!dota";   // ключ для шифрации
		const string REFRESH_KEY = "REFRESH_TOKEN_KEY!minecraft!asus!lenovo!honor!oracle";   // ключ для шифрации
		public static SymmetricSecurityKey GetSymmetricSecurityAccessKey() =>
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ACCESS_KEY));
		public static SymmetricSecurityKey GetSymmetricSecurityRefreshKey() =>
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(REFRESH_KEY));
	}
}
