using AspNetCoreWebApiLab.Api.Services;
using AspNetCoreWebApiLab.Persistence.DataTransferObjects;
using AspNetCoreWebApiLab.Persistence.EntityFrameworkContexts;
using AspNetCoreWebApiLab.Persistence.Mappers;
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
using System.Collections.Generic;
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
                swaggerOptions.SwaggerDoc("IdentityAPI-V1.0", GetApiInfo(versionNumber: "1.0", GetApiDescriptionV1()));
                swaggerOptions.SwaggerDoc("IdentityAPI-V2.0", GetApiInfo(versionNumber: "2.0", GetApiDescriptionV2()));
                swaggerOptions.SwaggerDoc("IdentityAPI-V3.0", GetApiInfo(versionNumber: "3.0", GetApiDescriptionV3()));

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                
                swaggerOptions.IncludeXmlComments(xmlCommentsFullPath);

                swaggerOptions.AddSecurityDefinition("jwtAuth", GetOpenApiSecurityDefinition());
                swaggerOptions.AddSecurityRequirement(GetOpenApiSecurityRequirement());
            });

            services.AddTransient<UserService>();
            services.AddTransient<RoleService>();
            services.AddTransient<UserRoleService>();
            services.AddTransient<RoleClaimService>();
            services.AddTransient<UserClaimService>();
            services.AddTransient<JwtService>();
            services.AddTransient<RoleDataMapper>();

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
                c.SwaggerEndpoint("/swagger/IdentityAPI-V3.0/swagger.json", "ASP.NET Core Identity API 3.0");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthentication();

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

        private OpenApiInfo GetApiInfo(string versionNumber, string ApiDescription)
        {
            return new OpenApiInfo()
            {
                Title = $"ASP.NET Core Identity API {versionNumber}",
                Version = $"{versionNumber}",
                Description = ApiDescription,
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

        private string GetApiDescriptionV1()
        {
            return @"Through this API you can access ASP.NET Core Identity services. 
                     The 1.0 version has the mainly resources, authentication and authorization features.";
        }

        private string GetApiDescriptionV2()
        {
            return @"Through this API you can access ASP.NET Core Identity services.
                     The 2.0 version has all features of version 1.0 plus asynchronous endpoints.";
        }

        private string GetApiDescriptionV3()
        {
            return @"Through this API you can access ASP.NET Core Identity services.
                     The 3.0 version has all features of version 1.0 and 2.0 plus caching, sorting, pagination,
                     filtering and data shaping.";
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

        private OpenApiSecurityScheme GetOpenApiSecurityDefinition()
        {
            return new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Name = "Authorization",
                Description = "Please insert JWT with Bearer into field"
            };
        }

        private OpenApiSecurityRequirement GetOpenApiSecurityRequirement()
        {
            var openApiSecurityRequirement = new OpenApiSecurityRequirement();
            var openApiSecurityScheme = new OpenApiSecurityScheme() 
            { 
                Reference = new OpenApiReference() { Type = ReferenceType.SecurityScheme, Id = "jwtAuth" } 
            };

            openApiSecurityRequirement.Add(openApiSecurityScheme, new List<string>());

            return openApiSecurityRequirement;
        }
    }
}
