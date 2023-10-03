﻿using course_work_server.Dto;
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
			User user = db.Users.FirstOrDefault(user => user.Login == loginDTO.Login && 
			    HashPasswordService.Compare(user.Password, loginDTO.Password));
			if (user == null) 
			{
				return "Неверный логин или пароль";
			}
			return null;

			
		}

	}
}