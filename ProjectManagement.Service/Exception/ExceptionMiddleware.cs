using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace ProjectManagement.Service.Exception
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ProjectManagementException ex)
            {
                await HandleExceptionAsync(context, ex.Code, ex.Message);
            }
            catch (System.Exception ex)
            {
                await HandleExceptionAsync(context, (int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, int code, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;

            var response = new
            {
                statusCode = code,
                error = message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
