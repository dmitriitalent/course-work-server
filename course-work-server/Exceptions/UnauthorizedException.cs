namespace course_work_server.Exceptions;

public class UnauthorizedException<TController> : ResponseException<TController>
{
	public UnauthorizedException(string message)
		: base(message, 401)
	{ }
}
