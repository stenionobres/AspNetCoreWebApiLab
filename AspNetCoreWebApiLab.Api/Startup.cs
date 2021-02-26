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
                swaggerOptions.SwaggerDoc("IdentityAPI", 
                                          new OpenApiInfo() 
                                          { 
                                            Title = "ASP.NET Core Identity API",
                                            Version = "1.0"
                                          });
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
    }
}
