using AspNetCoreWebApiLab.Api.Services;
using AspNetCoreWebApiLab.Persistence.DataTransferObjects;
using AspNetCoreWebApiLab.Persistence.EntityFrameworkContexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;
using System.Text;

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
                    })
                    .ConfigureApiBehaviorOptions(apiOptions => 
                    {
                        apiOptions.InvalidModelStateResponseFactory = context => { return CreateResponseFactory(context); };
                    });

            services.AddApiVersioning(versioningOptions => GetApiVersioningOptions(versioningOptions));

            services.AddSwaggerGen(swaggerOptions => 
            {
                swaggerOptions.SwaggerDoc("IdentityAPI-V1.0", GetApiInfo(versionNumber: "1.0"));
                swaggerOptions.SwaggerDoc("IdentityAPI-V2.0", GetApiInfo(versionNumber: "2.0"));

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                
                swaggerOptions.IncludeXmlComments(xmlCommentsFullPath);
            });

            services.AddTransient<UserService>();
            services.AddTransient<RoleService>();
            services.AddTransient<UserRoleService>();
            services.AddTransient<RoleClaimService>();
            services.AddTransient<UserClaimService>();
            services.AddTransient<JwtService>();

            services.AddDbContext<AspNetCoreWebApiLabDbContext>();

            services.AddIdentity<User, IdentityRole<int>>()
                    .AddEntityFrameworkStores<AspNetCoreWebApiLabDbContext>();

            services.AddAuthentication(authenticationOptions => { GetAuthenticationOptions(authenticationOptions); })
                    .AddJwtBearer(jwtBearerOptions => { GetJwtBearerOptions(jwtBearerOptions); });
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
                c.SwaggerEndpoint("/swagger/IdentityAPI-V2.0/swagger.json", "ASP.NET Core Identity API 2.0");
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

        private IActionResult CreateResponseFactory(ActionContext context)
        {
            // create a problem details object
            var problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);

            // add additional info not added by default
            problemDetails.Detail = "See the errors field for details";
            problemDetails.Instance = context.HttpContext.Request.Path;

            // find out which status code to use
            var actionExecutingContext = context as ActionExecutingContext;

            // if there are modelstate errors & all arguments were correctly 
            // found/parsed we are dealing with validation errors
            if (context.ModelState.ErrorCount > 0 && actionExecutingContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count)
            {
                problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                problemDetails.Title = "One or more validation errors occurred.";

                return new UnprocessableEntityObjectResult(problemDetails)
                {
                    ContentTypes = { "application/json" }
                };
            }

            // if one the arguments was not correctly found / could not parsed
            // we are dealing with null/unparseable input 
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "One or more errors on input occurred.";

            return new UnprocessableEntityObjectResult(problemDetails)
            {
                ContentTypes = { "application/json" }
            };
        }

        private void GetAuthenticationOptions(AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }

        private void GetJwtBearerOptions(JwtBearerOptions options)
        {
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"])),
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}
