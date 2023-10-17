namespace course_work_server.Exceptions;

public class ResponseException : Exception
{
	public ResponseException(string message, int statusCode, string? controller = null) 
		: base(message) 
	{
		this.StatusCode = statusCode;
		this.Controller = controller;
	}

	public int StatusCode { get; set; }
	public string? Controller { get; set; }
}

