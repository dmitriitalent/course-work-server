namespace course_work_server.Exceptions;

public class ValidationException : ResponseException
{
	public ValidationException(string message, string? controller = null) 
		: base (message, 403, controller) 
	{ }
}
