namespace course_work_server.Exceptions;

public class BadRequestException : ResponseException
{
	public BadRequestException(string message, string? controller = null)
		: base(message, 400, controller) 
	{ }
}
