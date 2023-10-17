namespace course_work_server.Exceptions;

public class ValidationException<TController> : ResponseException<TController>
{
	public ValidationException(string message) 
		: base (message, 403) 
	{ }
}
