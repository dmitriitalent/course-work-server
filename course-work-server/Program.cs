var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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


builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("AllCors");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
