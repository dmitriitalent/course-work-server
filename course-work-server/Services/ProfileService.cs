using course_work_server.Dto;
using course_work_server.Entities;
using Microsoft.AspNetCore.HttpLogging;

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

		public List<UserProfile> GetAll()
		{
			return db.UserProfiles.ToList();
		}
    }
}
