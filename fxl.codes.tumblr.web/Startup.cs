using fxl.codes.tumblr.web.Services;
using fxl.codes.tumblr.web.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql.Logging;

namespace fxl.codes.tumblr.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(Constants.AuthenticationScheme)
                .AddCookie(Constants.AuthenticationScheme, options => { options.LoginPath = "/Login"; });
            services.AddControllersWithViews(configure => { configure.Filters.Add(new AuthorizeFilter()); })
                .AddRazorRuntimeCompilation();
            
            services.AddSession();

            services.AddSingleton<TumblrService>();
            services.AddSingleton<UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Trace);
                NpgsqlLogManager.IsParameterLoggingEnabled = true;
            }

            app.UsePathBase(Configuration["PathBase"]);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}"); });

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
    }
}