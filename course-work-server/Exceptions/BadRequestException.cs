namespace course_work_server.Exceptions;

public class BadRequestException<TController> : ResponseException<TController>
{
	public BadRequestException(string message)
		: base(message, 400) 
	{ }
}
