using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Multilanguage.Application;
using Multilanguage.Infrastructure;
using System.Globalization;

namespace Multilanguage.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddHttpClient();
            services.AddInfrastructure(Configuration);
            services.AddApplication();
            services.AddControllers();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRequestLocalization(options =>
            {
                options.RequestCultureProviders.Clear();
                options.RequestCultureProviders.Add(new CookieRequestCultureProvider() { CookieName = CookieRequestCultureProvider.DefaultCookieName});
                options.SetDefaultCulture("en-GB");
                options.SupportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
                options.SupportedUICultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            });

            app.UseSwagger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseHttpsRedirection();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Multilanguage API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
