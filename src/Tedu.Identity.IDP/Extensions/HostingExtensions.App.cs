using Serilog;
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
            c.OAuthClientId(SystemConstants.TeduClients.DefaultClients.ClientId);
            c.SwaggerEndpoint(SystemConstants.Swagger.Url, SystemConstants.Swagger.EndpointName);
            c.DisplayRequestDuration();
        });

        app.UseRouting();

        // set cookie policy before auth authorization setup
        app.UseCookiePolicy();
        app.UseIdentityServer();

        // uncomment if you want to add a UI
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute().RequireAuthorization(SystemConstants.Swagger.Bearer);
            endpoints.MapRazorPages().RequireAuthorization();
        });

        return app;
    }
}
