using course_work_server.Dto;
using course_work_server.Entities;

namespace course_work_server.Services
{
	public class RegistrationService
	{
		DataContext db;
		ValidationService ValidationService;
		private HashPasswordService HashPasswordService;
		LoggerService LoggerService;
		public RegistrationService(DataContext db)
		{
			this.db = db;
			this.ValidationService = new ValidationService();
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
				Email = registrationDTO.Email,
				Login = registrationDTO.Login,
				Name = registrationDTO.Name,
				Surname = registrationDTO.Surname,
				Password = HashPasswordService.Make(registrationDTO.Password),
			};
			try
			{
				db.Users.Add(user);
				db.SaveChanges();
			}
			catch (Exception ex)
			{
				LoggerService.LogError(ex);
                return "Не удалось добавить пользователя в базу данных";
			}

			return null;
		}
		
	}
}
