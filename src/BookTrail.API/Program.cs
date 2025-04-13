using Asp.Versioning;
using Asp.Versioning.Builder;
using BookTrail.API;
using BookTrail.API.Data;
using BookTrail.API.Features;
using BookTrail.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateSlimBuilder(args);

// Add DbContext configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Contact = new OpenApiContact
        {
            Name = "BookTrail API",
            Email = "001knatz@gmail.com"
        };
        return Task.CompletedTask;
    });
});

var defaultVersion = new ApiVersion(1, 0);

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


var app = builder.Build();

// Create database and apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

// Define a set of supported API versions
var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(defaultVersion)
    .HasApiVersion(new ApiVersion(1, 1))
    .ReportApiVersions()
    .Build();


app.MapOpenApi();
app.MapScalarApiReference();

// Define the API root with a version placeholder
var apiRoot = app.MapGroup("/api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);
apiRoot.MapOpenApi();

apiRoot.MapV1TodosEndpoints();

await app.RunAsync();

