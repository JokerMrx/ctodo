namespace CTodo.Middlewares;

public class StorageTypeMiddleware
{
    private readonly RequestDelegate _next;

    public StorageTypeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("Database-type", out var storageType))
        {
            context.Items["StorageType"] = storageType.ToString();
        }

        await _next(context);
    }
}