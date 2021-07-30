using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using CoolParking.WebAPI.Models;
using Microsoft.AspNetCore.Http;

namespace CoolParking.WebAPI.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var errorResponse = new ErrorResponseData(response.StatusCode, "Internal Server Error", ex.Message);
                var errorJson = JsonSerializer.Serialize(errorResponse);
                await response.WriteAsync(errorJson);
            }
        }
    }
}
