using course_work_server.Dto;
using course_work_server.Entities;
using course_work_server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace course_work_server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RaceController : ControllerBase
{
    DataContext db;
    RaceService RaceService;
    
    public RaceController(DataContext db)
    {
        this.db = db;
        this.RaceService = new RaceService(this.db);
    }
    
    [HttpPost]
    [Route("create")]
    public IActionResult Create(RaceDTO raceDTO)
    {
        string error = RaceService.AddToDatabase(raceDTO);
        if (error != null) { return Problem(detail: error, statusCode: 500); }
        
        return Ok();
    }

    [HttpPost]
    [Route("update")]
    public IActionResult Update(int id, RaceDTO raceDTO)
    {
        string error = RaceService.UpdateInDatabase(id, raceDTO);
        if (error != null) { return Problem(detail: error, statusCode: 500); }
        
        return Ok();
    }
    
    [HttpPost]
    [Route("delete")]
    public IActionResult Delete(int id)
    {
        string error = RaceService.DeleteFromDatabase(id);
        if (error != null) { return Problem(detail: error, statusCode: 500); }
        
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