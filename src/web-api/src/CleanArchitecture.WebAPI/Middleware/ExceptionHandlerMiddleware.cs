using CleanArchitecture.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CleanArchitecture.WebAPI.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            catch (OperationNotAllowedForAccountTypeException ex)
            {
                LogException(ex);

                var statusCode = HttpStatusCode.InternalServerError;
                var result = JsonConvert.SerializeObject(Errors.User.OperationNotAllowedForSpecificUserProfileType(ex.AccountType));
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;
                await context.Response.WriteAsync(result);
            }

            catch (Exception ex)
            {
                LogException(ex);

                var statusCode = HttpStatusCode.InternalServerError;
                var result = JsonConvert.SerializeObject(new Error("Unexpected error has occurred",
                                                "Unfortunately unexpected error has occured. Please try again later"));
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;
                await context.Response.WriteAsync(result);
            }
        }

        private void LogException(Exception exception)
        {
            Log.Error("Unhandled exception was thrown:");
            Log.Error(exception.Message);
            Log.Error(exception.StackTrace);
        }
    }
}
