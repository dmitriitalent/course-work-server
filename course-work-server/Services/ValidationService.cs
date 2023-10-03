using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace course_work_server.Services
{
    public class ValidationService
    {
        public ValidationService() { }

        public string? ValidateLength(string word, int minLength = 0, double maxLength = double.PositiveInfinity)
        {
            if (word.Length < minLength)
            {
                return "меньше " + minLength + " символов";
            }
            if (word.Length > maxLength)
            {
                return "больше " + maxLength + " символов";
            }
            return null;
        }

        public string? ValidateEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (!match.Success)
            {
                return "Введите почтовый адресс";
            }
            else { return null; }
        }

        public string? ValidatePassword(string password)
        {
            if (password.Length < 7)
            {
                return "Слишком короткий пароль";
            }
            for (int i = 0; i < 10; i++)
            {
                if (password.Contains(i.ToString()))
                {
                    break;
                }
                if (i == 9)
                {
                    return "Пароль должен содержать цифру";
                }
            }
            return null;
        }

        public string? ValidatePasswordConfirm(string password, string passwordConfirm)
        {
            if (passwordConfirm != password)
                return "Пароли не совпадают";
            return null;
        }
    }
}
