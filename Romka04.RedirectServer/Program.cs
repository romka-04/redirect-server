using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Romka04.RedirectServer;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

app.Use(async (context, next) =>
{
    var method = context.Request.Method;

    if (method == "GET")
    {
        var username = context.Request.Headers["Remote-User"];

        if (username == StringValues.Empty)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync("\"HTTP Header 'Remote-User' is expected\"");
        }
        else
        {
            if (IsEqualOrdinalIgnoreCase(username, "roman"))
            {
                const string location = "http://heimdall.roman-assol.site";
                context.Response.Redirect(location);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.WriteAsync($"\"Redirect rule for Remote-User '{username}' has not found\"");
            }
        }
    }
    else
    {
        await next(context);
    }
});

bool IsEqualOrdinalIgnoreCase(StringValues x, string segment)
{
    var y = new StringValues(segment);
    return StringValuesComparer.OrdinalIgnoreCase.Equals(x, y);
}

await app.RunAsync();

// Make the implicit Program class public so test projects can access it
public partial class Program { }