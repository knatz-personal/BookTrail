using Asp.Versioning;

namespace BookTrail.API.Features
{
    public static class TodoEndpointDefinitions
    {
        public static void MapV1TodosEndpoints(this RouteGroupBuilder @this)
        {
            ArgumentNullException.ThrowIfNull(@this, nameof(@this));
            var group = @this.MapGroup("Todos")
                .WithTags("Todos")
                .WithDisplayName("Todos")
                .MapToApiVersion(new ApiVersion(1, 0));


            var sampleTodos = new Todo[]
            {
                        new(1, "Walk the dog"),
                        new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
                        new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
                        new(4, "Clean the bathroom"),
                        new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
            };

            group.MapGet("", () => sampleTodos);
            group.MapGet("{id}", (int id) =>
                sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
                    ? Results.Ok(todo)
                    : Results.NotFound());
        }
    }
}
