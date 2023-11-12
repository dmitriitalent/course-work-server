using course_work_server.Dto;
using course_work_server.Entities;
using course_work_server.Exceptions;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace course_work_server.Services
{
	public class ProfileService
	{
		DataContext db;
		LoggerService LoggerService;
        public ProfileService( DataContext db, LoggerService LoggerService) 
		{
			this.db = db;
			this.LoggerService = LoggerService;
        }

		public string? UpdateInDatabase(UserProfile newProfile)
		{
			try
			{
				UserProfile oldProfile = db.UserProfiles.FirstOrDefault(profile => profile.Id == newProfile.Id);
				if (oldProfile != null)
				{
					// Цикл пробегает по всем полям класса UserProfile. 
					foreach(var field in oldProfile.GetType().GetFields())
					{
						//filed.GetValue(из какого объекта взять значение поля);
						//field.SetValue(в какой объект вставляем, что вставляем)
						field.SetValue(oldProfile, field.GetValue(newProfile));
					}
					db.UserProfiles.Update(oldProfile);
					db.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				LoggerService.LogError(ex);
				return "Не удалось обновить данные";
			}

			return null;
		}
		public string GetUserById(int id)
		{
			User user = db.Users.Include(u => u.Profile).FirstOrDefault(u => u.Id == id);

			if (user == null)
			{
				throw new NotFoundException<ProfileService>("Пользователь с таким идентификатором не найден");
			}
			user.Profile.User = null;
			// Return profile data from user.Profile
			return JsonConvert.SerializeObject(
				user.Profile,
				new JsonSerializerSettings()
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				}
			);
		}
		public List<UserProfile> GetAll()
		{
			return db.UserProfiles.ToList();
		}
    }
}
