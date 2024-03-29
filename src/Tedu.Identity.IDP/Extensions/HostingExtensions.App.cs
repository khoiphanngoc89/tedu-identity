﻿using Serilog;
using Tedu.Identity.IDP.Middlewares;
using Tedu.Identity.Infrastructure.Const;

namespace Tedu.Identity.IDP.Extensions;

internal static partial class HostingExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // uncomment if you want to add a UI
        app.UseStaticFiles();
        app.UseCors(SystemConstants.ConfigureOptions.CorsPolicy);

        // swagger
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.OAuthClientId(SystemConstants.SwaggerClients.ClientId);
            c.SwaggerEndpoint(SystemConstants.ConfigureOptions.Url, SystemConstants.ConfigureOptions.EndpointName);
            c.DisplayRequestDuration();
        });

        app.UseRouting();

        app.UseMiddleware<ErrorWrappingMiddleware>();

        // set cookie policy before auth authorization setup
        app.UseCookiePolicy();
        app.UseIdentityServer();

        // uncomment if you want to add a UI
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute().RequireAuthorization(SystemConstants.ConfigureOptions.Bearer);
            endpoints.MapRazorPages().RequireAuthorization();
        });

        return app;
    }
}
