using Asp.Versioning;
using BookTrail.API;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateSlimBuilder(args);

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

// Add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

var apiRoot = app.MapGroup("/api");
apiRoot.MapOpenApi();

// Version 1.0 routes
var v1 = apiRoot.MapGroup("/v1");

MapTodosEndpoints(v1);

await app.RunAsync();


// Helper method to map todos endpoints
void MapTodosEndpoints(RouteGroupBuilder group)
{
    var sampleTodos = new Todo[]
    {
        new(1, "Walk the dog"),
        new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
        new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
        new(4, "Clean the bathroom"),
        new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
    };

    group.MapGet("/todos", () => sampleTodos);
    group.MapGet("/todos/{id}", (int id) =>
        sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
            ? Results.Ok(todo)
            : Results.NotFound());
}
