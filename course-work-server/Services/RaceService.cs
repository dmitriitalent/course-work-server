using course_work_server.Dto;
using course_work_server.Entities;
using Microsoft.EntityFrameworkCore;

namespace course_work_server.Services;

public class RaceService
{
    DataContext db;
    LoggerService LoggerService;

    public RaceService(DataContext db)
    {
        this.db = db;
    }

    public string? AddToDatabase(RaceDTO raceDTO)
    {
        Race race = new Race();

		foreach (var field in raceDTO.GetType().GetFields())
		{
			//filed.GetValue(из какого объекта взять значение поля);
			//field.SetValue(в какой объект вставляем, что вставляем)
			field.SetValue(race, field.GetValue(raceDTO));
		}
        race.Date = DateTime.Parse(raceDTO.Date);

		try
        {
            db.Races.Add(race);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            LoggerService.LogError(ex);
            return "Не удалось добавить событие";
        }
        
        return null;
    }
    
    public string? UpdateInDatabase(int id, RaceDTO raceDTO)
    {
        try
        {
            Race race = db.Races.FirstOrDefault(race => race.Id == id);
            if (race != null)
            {

				foreach (var field in raceDTO.GetType().GetFields())
				{
					//filed.GetValue(из какого объекта взять значение поля);
					//field.SetValue(в какой объект вставляем, что вставляем)
					field.SetValue(race, field.GetValue(raceDTO));
				}
				db.Races.Update(race);
                db.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            LoggerService.LogError(ex);
            return "Не удалось обновить событие";
        }
        
        return null;
    }

    public string? DeleteFromDatabase(int id)
    {
        try
        {
            Race race = db.Races.FirstOrDefault(race => race.Id == id);
            if (race != null)
            {
                db.Races.Remove(race);
                db.SaveChanges();
            }
            else
            {
                return "Событие с таким id не найдено";
            }
        }
        catch (Exception ex)
        {
            LoggerService.LogError(ex);
            return "Не удалось удалить событие";
        }

        return null;
    }
    
    public Race GetFromDatabase(int id)
    {
        try
        {
            Race race = db.Races.FirstOrDefault(race => race.Id == id);
            return race;
        }
        catch (Exception ex)
        {
            LoggerService.LogError(ex);
        }
        
        return null;
    }

    public List<Race>? GetAllFromDatabase()
    {
        try
        {
            var races = db.Races.FromSqlRaw("SELECT * FROM Races").ToList();
            return races;
        }
        catch (Exception ex)
        {
            LoggerService.LogError(ex);
        }
        
        return null;
    }
}