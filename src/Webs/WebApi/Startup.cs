using System.Reflection;
using DialogWeaver.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NSwag.AspNetCore;

namespace DialogWeaver.WebApi
{
    /// <summary>
    /// Startup of the ASP.Net application
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddCors( app => app.AddPolicy( "default",
                options => options.AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowAnyOrigin() ) );

            services.AddTransient<IValueService, ValueService>();
        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The environment.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("default");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var swaggerUiSettings = new SwaggerUiSettings
            {
                IsAspNetCore = true,
                SupportedSubmitMethods = new string[0],

            };
            app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, swaggerUiSettings);

            app.UseMvcWithDefaultRoute();
        }
    }
}
