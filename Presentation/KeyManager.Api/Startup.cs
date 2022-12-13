using System.IO;
using System.Reflection;
using System.Text;
using KeyManager.Api.Security.Handlers;
using KeyManager.Api.Security.Requirements;
using KeyManager.Application;
using Microsoft.IdentityModel.Tokens;

namespace KeyManager.Api;

/// <summary>
///     Application startup
/// </summary>
[ExcludeFromCodeCoverage]
public class Startup
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    ///     Configuration
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    ///     Configure services
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication();
        services.AddDbContextPool<DataContext>(options =>
            options.UseSqlite(Configuration.GetConnectionString("KeyManagerDb")));

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        //Can be replaced with a real db repository
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Enter user token created from Login endpoint.",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() }
            });
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        #region Authentication

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            };
        });

        #endregion

        #region Authorization

        services.AddAuthorization(options =>
        {
            options.AddPolicy(nameof(AuthorizationRequirement),
                policy => policy.Requirements.Add(new AuthorizationRequirement()));
            options.AddPolicy(nameof(SystemManagerRequirement),
                policy => policy.Requirements.Add(new SystemManagerRequirement()));
            options.AddPolicy(nameof(KnownRolesRequirement),
                policy => policy.Requirements.Add(new KnownRolesRequirement()));
            options.AddPolicy(nameof(UserManagerRequirement),
                policy => policy.Requirements.Add(new UserManagerRequirement()));
        });
        services.AddHttpContextAccessor();
        services.AddScoped<IAuthorizationHandler, AuthorizationHandler>();
        services.AddScoped<IAuthorizationHandler, WebsiteAccessAuthorizationHandler>();

        #endregion
    }

    /// <summary>
    ///     Configure application
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    /// <param name="dataContext"></param>
    /// <param name="autoMapperConfiguration"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext dataContext,
        IConfigurationProvider autoMapperConfiguration)
    {
        if (env.IsDevelopment())
        {
            autoMapperConfiguration.AssertConfigurationIsValid();

            app.UseDeveloperExceptionPage();
        }

        dataContext.Database.Migrate();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = "swagger";
            options.SwaggerEndpoint("v1/swagger.json", "KeyManager API v1");
        });
    }
}