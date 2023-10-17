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

[Route("api/[controller]")]
[ApiController]
public class ProfileController : ControllerBase
{

	DataContext db;
	TokenService TokenService;
	AuthorizeService AuthorizeService;
	ProfileService ProfileService;
	LoggerService LoggerService;
    public ProfileController(DataContext db)
	{
		this.db = db;
		this.TokenService = new TokenService(db);
		this.AuthorizeService = new AuthorizeService(Request, this.TokenService);
		this.LoggerService = new LoggerService();
        this.ProfileService = new ProfileService(db, this.LoggerService);

    }

	[HttpPost]
	[Route("get")]
	public IActionResult Get()
	{
		// Check if user is`n authorized
		if (!AuthorizeService.IsAuthorize())
		{
			throw new UnauthorizedException<ProfileController>("Пользователь не авторизован");
		}

		// Get user by token
		string token = null;
		Request.Cookies.TryGetValue("RefreshToken", out token);
		User user = TokenService.GetUserByToken(token);

		// Return profile data from user.Profile
		return Ok(
			JsonConvert.SerializeObject(
				user.Profile,
				new JsonSerializerSettings()
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				}
			)
		);
	}

	[HttpPost]
	[Route("getAll")]
	public IActionResult GetAll() 
	{
		return Ok(ProfileService.GetAll());
	}

    [HttpPost]
    [Route("Update")]
    public IActionResult Update(UserProfile profile)
    {
        string error = ProfileService.UpdateInDatabase(profile);
        if (error != null) { throw new InternalServerException<ProfileController>(error); }

        return Ok();
    }
}
