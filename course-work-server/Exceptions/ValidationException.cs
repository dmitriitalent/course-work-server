namespace course_work_server.Exceptions;

public class ValidationException<TController> : ResponseException
{
	public ValidationException(string message) 
		: base (message, 403, typeof(TController).Name) 
	{ }
}
