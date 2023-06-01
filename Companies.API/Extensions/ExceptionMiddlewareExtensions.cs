using Companies.API.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace Companies.API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        private static ProblemDetailsFactory problemDetailsFactory;

        public static void ConfigureExceptionHandler(this WebApplication app)
        {
            var logger = app.Services.GetRequiredService<ILogger<Exception>>();
            problemDetailsFactory = app.Services.GetRequiredService<ProblemDetailsFactory>();

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"Exception middleware catches an exception: {contextFeature.Error}");

                        var problemDetails = GetErrorResponse(context, contextFeature, out int statusCode);

                        context.Response.StatusCode = statusCode;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(problemDetails);
                    }
                });
            });
        }

        private static object GetErrorResponse(HttpContext context, IExceptionHandlerFeature contextFeature, out int statusCode)
        {
            object problemDetails = default!;

            switch (contextFeature.Error)
            {
                case CompanyNotFoundException companyNotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    problemDetails = problemDetailsFactory.CreateProblemDetails(
                                                                    context,
                                                                    statusCode,
                                                                    companyNotFoundException.Message);
                    break;
                case BadRequestException badRequestException:
                    statusCode = StatusCodes.Status422UnprocessableEntity;
                    var problems = problemDetailsFactory.CreateProblemDetails(
                                                                   context,
                                                                   statusCode,
                                                                   badRequestException.Message);
                    problemDetails = new
                    {
                        errors = badRequestException.Errors,
                        details = problems
                    };
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    problemDetails = problemDetailsFactory.CreateProblemDetails(
                                                                    context,
                                                                    statusCode,
                                                                    "Internal Server Error");
                    break;
            }

            return problemDetails;
        }
    }
}
