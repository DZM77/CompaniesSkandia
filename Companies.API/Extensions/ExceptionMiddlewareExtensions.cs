using Companies.API.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace Companies.API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app)
        {
            var logger = app.Services.GetRequiredService<ILogger<Exception>>();
            var problemDetailsFactory = app.Services.GetRequiredService<ProblemDetailsFactory>();

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    //context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"Exception middleware catches an exception: {contextFeature.Error}");

                        ProblemDetails problemDetails = default!;
                        int statusCode;

                        switch (contextFeature.Error)
                        {
                            case CompanyNotFoundException companyNotFoundException:
                                statusCode = StatusCodes.Status404NotFound;
                                problemDetails = problemDetailsFactory.CreateProblemDetails(
                                                                                context,
                                                                                statusCode,
                                                                                companyNotFoundException.Message);
                                break;
                            default:
                                statusCode = StatusCodes.Status500InternalServerError;
                                problemDetails = problemDetailsFactory.CreateProblemDetails(
                                                                                context,
                                                                                statusCode,
                                                                                "Internal Server Error");
                                break;
                        }


                        context.Response.StatusCode = statusCode;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(problemDetails);
                    }
                });
            });
        }
    }
}
