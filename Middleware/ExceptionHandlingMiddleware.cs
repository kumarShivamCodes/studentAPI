using System.Net;
using System.Text.Json;

public class ExceptionHandlingMiddleware
{
  private readonly RequestDelegate _next;
  public ExceptionHandlingMiddleware(RequestDelegate next)
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
      await HandleExceptionAsync(context, ex);
    }
  }

  private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      context.Response.ContentType = "application/json";
      var response = context.Response;

      var errorResponse = new ErrorResponse
      {
        Success = false
      };

      switch (exception)
      {
        case KeyNotFoundException ex:
          response.StatusCode = (int)HttpStatusCode.NotFound;
          errorResponse.Message = ex.Message;
          break;

        case ArgumentException ex:
          response.StatusCode = (int)HttpStatusCode.BadRequest;
          errorResponse.Message = ex.Message;
          break;

        case InvalidOperationException ex:
          response.StatusCode = (int)HttpStatusCode.BadRequest;
          errorResponse.Message = ex.Message;
          break;  

        default:
          response.StatusCode = (int) HttpStatusCode.InternalServerError;
          errorResponse.Message = "Internal server error!";
          break;    
      }
      var result = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(result);
        
    }
}

internal class ErrorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
}