// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Middleware;

namespace ExtDotNet.OAuth20.Server;

public static class OAuth20ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseOAuth20Server(this IApplicationBuilder app)
    {
        app.UseMiddleware<OAuth20ServerEndpointsMiddleware>();
        app.UseMiddleware<OAuth20ServerWebPagesMiddleware>();

        return app;
    }
}
