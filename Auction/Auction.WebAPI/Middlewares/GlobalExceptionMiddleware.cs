using System.Net;
using Auction.Common.Response;
using Newtonsoft.Json;

namespace Auction.WebAPI.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = new Response<string>()
            {
                Message = ex.Message,
                Status = Status.Error
            };

            await response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }
}
