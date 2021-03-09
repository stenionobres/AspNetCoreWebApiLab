using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;

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
            services.AddControllers()
                    .AddNewtonsoftJson(newtonSoftOptions =>
                    {
                        newtonSoftOptions.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    });

            services.AddApiVersioning(versioningOptions => GetApiVersioningOptions(versioningOptions));

            services.AddSwaggerGen(swaggerOptions => 
            {
                swaggerOptions.SwaggerDoc("IdentityAPI-V1.0", GetApiInfo(versionNumber: "1.0"));

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                
                swaggerOptions.IncludeXmlComments(xmlCommentsFullPath);
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
                c.SwaggerEndpoint("/swagger/IdentityAPI-V1.0/swagger.json", "ASP.NET Core Identity API 1.0");
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

        private OpenApiInfo GetApiInfo(string versionNumber)
        {
            return new OpenApiInfo()
            {
                Title = $"ASP.NET Core Identity API {versionNumber}",
                Version = $"{versionNumber}",
                Description = "Through this API you can access ASP.NET Core Identity services",
                Contact = new OpenApiContact()
                {
                    Name = "Stenio Nobres",
                    Url = new Uri("https://www.linkedin.com/in/stenionobres/")
                },
                License = new OpenApiLicense()
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            };
        }
    }
}
