using DotNetEnv;
using Hellang.Middleware.ProblemDetails;
using MangaScans.Api.Extensions;
using MangaScans.Api.IoC;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
	Env.Load();
	builder.Services.AddSwaggerSetup();
}

builder.Services.AddControllers();
builder.Services.AddProblemDetailsSetup();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthenticationSetup(builder.Configuration);
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

app.UseCors(x => x
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader());

app.UseHttpsRedirection();

await app.UseAutenticationSetup();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseSwaggerSetup();
app.UseProblemDetails();
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = 
		new PhysicalFileProvider(
			Path.Combine(app.Environment.ContentRootPath, "StaticFiles")),
	RequestPath = "/Images"
});

app.Run();
