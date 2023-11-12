using course_work_server.Dto;
using course_work_server.Entities;
using course_work_server.Exceptions;
using course_work_server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace course_work_server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RaceController : ControllerBase
{
    DataContext db;
    RaceService RaceService;
    TokenService TokenService;
    AuthService AuthService;
    
    public RaceController(DataContext db)
    {
        this.db = db;
        this.RaceService = new RaceService(this.db);
        this.TokenService = new TokenService(this.db);
        this.AuthService = new AuthService(this.TokenService);
    }
    
    [HttpPost]
    [Route("create")]
    public IActionResult Create(RaceDTO raceDTO)
    {
        Console.WriteLine(raceDTO.Date);
		string refreshTokenString = null;
		Request.Cookies.TryGetValue("RefreshToken", out refreshTokenString);
		if (!AuthService.IsAdmin(refreshTokenString))
		{
            throw new ForbiddenException<RaceController>();
        }

        string error = RaceService.AddToDatabase(raceDTO);
        if (error != null) { throw new InternalServerException<RaceController>(error); }
        
        return Ok();
    }

    [HttpPost]
    [Route("update")]
    public IActionResult Update(int id, RaceDTO raceDTO)
    {
		string refreshTokenString = null;
		Request.Cookies.TryGetValue("RefreshToken", out refreshTokenString);
		if (!AuthService.IsAdmin(refreshTokenString))
		{
			throw new ForbiddenException<RaceController>();
		}

		string error = RaceService.UpdateInDatabase(id, raceDTO);
        if (error != null) { throw new InternalServerException<RaceController>(error); }
        
        return Ok();
    }
    
    [HttpPost]
    [Route("delete")]
    public IActionResult Delete(int id)
    {
		string refreshTokenString = null;
		Request.Cookies.TryGetValue("RefreshToken", out refreshTokenString);
		if (!AuthService.IsAdmin(refreshTokenString))
		{
			throw new ForbiddenException<RaceController>();
		}

		string error = RaceService.DeleteFromDatabase(id);
        if (error != null) { throw new InternalServerException<RaceController>(error); }
        
        return Ok();
    }

    [HttpPost]
    [Route("get")]
    public IActionResult Get(int id)
    {
        return Ok(RaceService.GetFromDatabase(id));
    }

    [HttpPost]
    [Route("getAll")]
    public IActionResult GetAll()
    {
        return Ok(RaceService.GetAllFromDatabase());
    }
    
}