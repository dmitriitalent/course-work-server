namespace course_work_server.Exceptions;

public class UnauthorizedException : ResponseException
{
	public UnauthorizedException(string message, string? controller = null)
		: base(message, 401, controller)
	{ }
}
