using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AspNetCoreWebApiLab.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddApiVersioning(versioningOptions => GetApiVersioningOptions(versioningOptions));

            services.AddSwaggerGen(swaggerOptions => 
            {
                swaggerOptions.SwaggerDoc("IdentityAPI", GetApiInfo());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/IdentityAPI/swagger.json", "ASP.NET Core Identity API");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void GetApiVersioningOptions(ApiVersioningOptions apiVersioningOptions)
        {
            apiVersioningOptions.AssumeDefaultVersionWhenUnspecified = true;
            apiVersioningOptions.ReportApiVersions = true;
            apiVersioningOptions.DefaultApiVersion = new ApiVersion(1, 0);
            apiVersioningOptions.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader(),
                                                                             new HeaderApiVersionReader("X-Version"),
                                                                             new UrlSegmentApiVersionReader());
        }

        private OpenApiInfo GetApiInfo()
        {
            return new OpenApiInfo()
            {
                Title = "ASP.NET Core Identity API",
                Version = "1.0",
                Description = "Through this API you can access ASP.NET Core Identity services",
                Contact = new OpenApiContact()
                {
                    Name = "Stenio Nobres",
                    Url = new System.Uri("https://www.linkedin.com/in/stenionobres/")
                },
                License = new OpenApiLicense()
                {
                    Name = "MIT License",
                    Url = new System.Uri("https://opensource.org/licenses/MIT")
                }
            };
        }
    }
}
