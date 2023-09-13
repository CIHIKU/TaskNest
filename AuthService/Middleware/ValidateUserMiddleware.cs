using AuthService.Services;

namespace AuthService.Middleware;

public class ValidateUserMiddleware
{
    private readonly RequestDelegate _next;

    public ValidateUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IJwtService jwtService)
    {
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        
        if (ipAddress != null)
        {
            context.Items["IPAddress"] = ipAddress;
        }
        
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token == null)
            await _next(context);

        var claimsPrincipal = jwtService.ValidateToken(token!);

        if (claimsPrincipal == null)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid token");
            return;
        }

        context.User = claimsPrincipal;

        await _next(context);
    }
}