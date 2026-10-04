public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KeyNotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;

            await context.Response.WriteAsJsonAsync(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;

            await context.Response.WriteAsJsonAsync(new
            {
                message = ex.Message
            });
        }
    }
}