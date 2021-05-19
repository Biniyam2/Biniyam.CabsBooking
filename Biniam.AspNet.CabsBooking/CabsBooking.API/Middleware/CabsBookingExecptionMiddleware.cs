using ApplicationCore.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CabsBooking.API.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CabsBookingExecptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CabsBookingExecptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleException(httpContext, e);
            }
        }


        public async Task HandleException(HttpContext httpContext, Exception ex)
        {
            switch (ex)
            {
                case ConflictException conflictException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    break;
                case NotFoundException notFoundException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case UnauthorizedAccessException unauthorized:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case Exception exception:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;

            }
        }


    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CabsBookingExecptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCabsBookingExecptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CabsBookingExecptionMiddleware>();
        }
    }
}
