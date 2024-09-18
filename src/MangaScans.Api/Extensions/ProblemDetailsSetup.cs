using System.Data.Common;
using Hellang.Middleware.ProblemDetails;
using MangaScans.Data.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Api.Extensions;

public static class ProblemDetailsSetup
{
    public static void AddProblemDetailsSetup(this IServiceCollection services)
    {
        services.AddProblemDetails(x =>
        {
            x.IncludeExceptionDetails = (ctx, exception) =>
            {
                var env = ctx.RequestServices.GetRequiredService<IHostEnvironment>();
                return env.IsDevelopment();
            };
            
            x.MapCustomStatusCode<DbEntityException>(StatusCodes.Status404NotFound);
            x.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
            x.MapToStatusCode<ArgumentException>(StatusCodes.Status400BadRequest);
            x.MapToStatusCode<UnauthorizedAccessException>(StatusCodes.Status401Unauthorized);
            x.MapToStatusCode<KeyNotFoundException>(StatusCodes.Status404NotFound);
            x.MapToStatusCode<DbException>(StatusCodes.Status500InternalServerError);
            x.MapToStatusCode<InvalidOperationException>(StatusCodes.Status409Conflict);
            x.MapToStatusCode<DbUpdateException>(StatusCodes.Status500InternalServerError);
            x.MapToStatusCode<FileNotFoundException>(StatusCodes.Status404NotFound);
            x.MapToStatusCode<NotSupportedException>(StatusCodes.Status405MethodNotAllowed);
            x.MapToStatusCode<TaskCanceledException>(StatusCodes.Status408RequestTimeout);
            x.MapToStatusCode<InvalidOperationException>(StatusCodes.Status401Unauthorized);
            x.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        });
    }

    public static void MapCustomStatusCode<TExeption>
        (this Hellang.Middleware.ProblemDetails.ProblemDetailsOptions options, int statusCode) where TExeption : Exception
    {
        options.Map<TExeption>(ex => new StatusCodeProblemDetails(statusCode)
        {
            Detail = ex.Message
        });
    }
}