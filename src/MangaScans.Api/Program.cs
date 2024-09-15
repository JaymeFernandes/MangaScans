using Hellang.Middleware.ProblemDetails;
using MangaScans.Api.Extensions;
using MangaScans.Api.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerSetup();
builder.Services.AddProblemDetailsSetup();
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.MapControllers();
app.UseSwaggerSetup();
app.UseProblemDetails();

app.Run();
