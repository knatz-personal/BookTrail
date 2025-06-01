using System.Text;
using Asp.Versioning;
using Asp.Versioning.Builder;
using BookTrail.API.Auth;
using BookTrail.Data;
using BookTrail.Data.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);

// Add configuration
builder.Services.Configure<AuthOptions>(
    builder.Configuration.GetSection(AuthOptions.ConfigurationSection));

// Add DbContext configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity
builder.Services.AddIdentityCore<User>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddSignInManager();

// Add Authentication
AuthOptions authOptions = builder.Configuration
    .GetSection(AuthOptions.ConfigurationSection)
    .Get<AuthOptions>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = authOptions?.Jwt.Issuer,
            ValidAudience = authOptions?.Jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(authOptions?.Jwt.Secret ?? string.Empty))
        };
    });

builder.Services.ConfigureHttpJsonOptions(_ =>
{
    // Removed AppJsonSerializerContext.Default as it is not defined
    // options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

// Add OpenAPI with authentication
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((openApiDoc, context, cancellationToken) =>
    {

        // Add JWT authentication to Swagger

        if (openApiDoc.Components == null)
        {
            openApiDoc.Components = new OpenApiComponents();
        }

        openApiDoc.Info.Contact = new OpenApiContact { Name = "BookTrail API", Email = "001knatz@gmail.com" };

        var bearerSecurityScheme =
            new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            };

        openApiDoc.Components
                .SecuritySchemes
                .Add("Bearer", bearerSecurityScheme);

        OpenApiSecurityRequirement bearerSecurityRequirement = new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            };

        openApiDoc.SecurityRequirements = new List<OpenApiSecurityRequirement>
        {
            bearerSecurityRequirement
        };

        return Task.CompletedTask;
    });
});

ApiVersion defaultVersion = new(1, 0);

// Add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = defaultVersion;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

WebApplication app = builder.Build();

// Create database and apply migrations on startup
using (IServiceScope scope = app.Services.CreateScope())
{
    ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

// Define a set of supported API versions
ApiVersionSet apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(defaultVersion)
    .HasApiVersion(new ApiVersion(1, 1))
    .ReportApiVersions()
    .Build();

app.UseAuthentication();
//app.UseAuthorization();

app.MapOpenApi();
app.MapScalarApiReference();

// Define the API root with a version placeholder
RouteGroupBuilder apiRoot = app.MapGroup("/api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

apiRoot.MapAuthEndpoints();

await app.RunAsync();