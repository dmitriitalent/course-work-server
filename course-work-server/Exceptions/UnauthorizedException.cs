namespace course_work_server.Exceptions;

public class UnauthorizedException<TController> : ResponseException
{
	public UnauthorizedException(string message)
		: base(message, 401, typeof(TController).Name)
	{ }
}
