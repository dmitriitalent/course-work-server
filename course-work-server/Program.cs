using Microsoft.EntityFrameworkCore;
using course_work_server.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using course_work_server.Services;

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

app.MapControllers();

app.Run();
