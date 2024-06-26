﻿using course_work_server.Dto;
using course_work_server.Entities;

namespace course_work_server.Services;

public class RegistrationService
{
	DataContext db;
	ValidationService ValidationService;
	HashPasswordService HashPasswordService;
	LoggerService LoggerService;
	public RegistrationService(DataContext db)
	{
		this.db = db;
		this.ValidationService = new ValidationService();
		this.HashPasswordService = new HashPasswordService();
	}

	public string? Validate(RegistrationDTO registrationDTO)
	{
		string error = null;
		error = ValidationService.ValidateEmail(registrationDTO.Email);
		if (error != null) 
			return error;
			
		error = ValidationService.ValidatePassword(registrationDTO.Password);
		if(error != null)
			return error;

		error = ValidationService.ValidatePasswordConfirm(registrationDTO.Password, registrationDTO.PasswordConfirmed);
		if(error != null)
			return error;

		error = ValidationService.ValidateLength(registrationDTO.Surname, 2);
		if (error != null)
			return "Введите фамилию";

		error = ValidationService.ValidateLength(registrationDTO.Name, 2);
		if (error != null)
			return "Введите имя";

		error = ValidationService.ValidateLength(registrationDTO.Login, 2, 20);
		if (error != null)
			return "Логин " + error;

		return null;
	}

	public string? CheckLogin(string login)
	{
		User user = db.Users.FirstOrDefault(user => user.Login == login);
		if (user != null) { return "Логин уже используется"; }
		return null;
	}

	public string? AddUserToDatabase(RegistrationDTO registrationDTO)
	{
		User user = new User()
		{
			Login = registrationDTO.Login,
			Password = HashPasswordService.Make(registrationDTO.Password),
			Salt = "",
			Role = Roles.User
		};
		try { db.Users.Add(user); }
		catch (Exception ex)
		{
			LoggerService.LogError(ex);
			return "Не удалось добавить пользователя в базу данных";
		}
		
		UserProfile userProfile = new UserProfile()
		{
			Email = registrationDTO.Email,
			Name = registrationDTO.Name,
			Surname = registrationDTO.Surname,
			User = user
		};
		try { db.UserProfiles.Add(userProfile); }
		catch (Exception ex)
		{
			LoggerService.LogError(ex);
			return "Не удалось добавить пользователя в базу данных";
		}

		try { db.SaveChanges(); }
		catch (Exception ex)
		{
			return "Не удалось сохранить пользователя";
		}
		return null;
	}
}

static class Roles
{
	public static string Admin = "Admin";
	public static string Moderator = "Moderator";
	public static string User = "User";
}