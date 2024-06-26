﻿using Microsoft.AspNetCore.Mvc;

namespace course_work_server.Exceptions;

public class InternalServerException<TController> : ResponseException
{
	public InternalServerException(string message)
		: base(message, 500, typeof(TController).Name)
	{ }
}
