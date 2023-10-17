namespace course_work_server.Exceptions;

public class ForbiddenException<TController> : ResponseException
{
	public ForbiddenException(string message = "Недостаточно прав")
		: base(message, 403, typeof(TController).Name)
	{ }
}