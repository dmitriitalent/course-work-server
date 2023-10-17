namespace course_work_server.Exceptions;

public class InternalServerException : ResponseException
{
	public InternalServerException(string message, string? controller = null)
		: base(message, 500, controller)
	{ }
}
