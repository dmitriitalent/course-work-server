namespace course_work_server.Exceptions;

public class ResponseException <TController> : Exception
{
	public ResponseException(string message, int statusCode) 
		: base(message) 
	{
		this.StatusCode = statusCode;
		this.Controller = typeof(TController).Name;
	}

	public int StatusCode { get; set; }
	public string? Controller { get; set; }
}

