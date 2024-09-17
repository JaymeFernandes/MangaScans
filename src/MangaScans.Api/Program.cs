using Hellang.Middleware.ProblemDetails;
using MangaScans.Api.Extensions;
using MangaScans.Api.IoC;
using MangaScans.Application.DTOs.Request;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerSetup();
builder.Services.AddProblemDetailsSetup();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.MapControllers();
app.UseSwaggerSetup();
app.UseProblemDetails();




app.Run();
