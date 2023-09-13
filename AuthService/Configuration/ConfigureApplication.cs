namespace AuthService.Configuration;

public static class ConfigureApplication
{
    public static void Configure(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Service v1"));
        
        app.UseHttpsRedirection();
        
        app.MapControllers();
    }
}