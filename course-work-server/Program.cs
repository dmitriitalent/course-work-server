using course_work_server.Services;
using course_work_server.Entities;
using course_work_server.Exceptions;
using course_work_server.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(connection));

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: "AllCors",
		policy =>
		{
			policy.WithOrigins("http://localhost:8081", "http://localhost:8080")
				  .AllowAnyMethod()
				  .AllowAnyHeader();
		});
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = TokenSettings.ISSUER,
			ValidateAudience = true,
			ValidAudience = TokenSettings.AUDIENCE,
			ValidateLifetime = true,
			IssuerSigningKey = TokenSettings.GetSymmetricSecurityAccessKey(),
			ValidateIssuerSigningKey = true,
		};
	});


builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("AllCors");


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
	try
	{
		await next.Invoke();
	}
	catch (ResponseException ex)
	{
		context.Response.StatusCode = ex.StatusCode;
		await context.Response.WriteAsync(ex.Message + " in " + ex.Controller);
	}
});

app.MapControllers();


app.Run();
