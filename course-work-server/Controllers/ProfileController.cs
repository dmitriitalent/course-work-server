using course_work_server.Entities;
using course_work_server.Services;
using course_work_server.Dto;
using course_work_server.Exceptions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.Security;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.OAuth;

[Route("api/[controller]")]
[ApiController]
public class ProfileController : ControllerBase
{

	DataContext db;
	TokenService TokenService;
	AuthService AuthService;
	ProfileService ProfileService;
	LoggerService LoggerService;
    public ProfileController(DataContext db)
	{
		this.db = db;
		this.TokenService = new TokenService(db);
		this.LoggerService = new LoggerService();
        this.ProfileService = new ProfileService(db, this.LoggerService);

    }

	[HttpPost]
	[Route("get")]
	public IActionResult Get(int id)
	{
		return Ok(ProfileService.GetUserById(id));
	}

	[HttpPost]
	[Route("getAll")]
	public IActionResult GetAll() 
	{
		return Ok(ProfileService.GetAll());
	}

    [HttpPost]
    [Route("Update")]
    public IActionResult Update(UserProfileDTO profileDTO)
    {
		// Проверяем верифицирован ли пользователь
		if(!AuthService.IsAuthenticated(Request.Cookies))
		{
			throw new UnauthorizedException<ProfileController>(); 
		}

		UserProfile profile = new UserProfile();
		foreach (var field in profileDTO.GetType().GetFields())
		{
			//filed.GetValue(из какого объекта взять значение поля);
			//field.SetValue(в какой объект вставляем, что вставляем)
			field.SetValue(profile, field.GetValue(profileDTO));
		}
		profile.Id = int.Parse(AuthService.GetClaimValue("Id", Request.Cookies));

		string error = ProfileService.UpdateInDatabase(profile);
        if (error != null) { throw new InternalServerException<ProfileController>(error); }

        return Ok();
    }
}
