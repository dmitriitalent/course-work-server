namespace course_work_server.Exceptions;

public class NotFoundException<TController> : ResponseException
{
	public NotFoundException(string message)
		: base(message, 404, typeof(TController).Name)
	{ }
}