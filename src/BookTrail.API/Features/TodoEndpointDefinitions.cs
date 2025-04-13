using Asp.Versioning;
using BookTrail.API.Data;
using BookTrail.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookTrail.API.Features;

public static class TodoEndpointDefinitions
{
    public static void MapV1TodosEndpoints(this RouteGroupBuilder @this)
    {
        ArgumentNullException.ThrowIfNull(@this, nameof(@this));
        var group = @this.MapGroup("Todos")
            .WithTags("Todos")
            .WithDisplayName("Todos")
            .MapToApiVersion(new ApiVersion(1, 0));

        group.MapGet("", async (ApplicationDbContext db) => 
            await db.Todos.ToListAsync());

        group.MapGet("{id}", async (int id, ApplicationDbContext db) =>
            await db.Todos.FindAsync(id) is Todo todo
                ? Results.Ok(todo)
                : Results.NotFound());

        group.MapPost("", async (Todo todo, ApplicationDbContext db) =>
        {
            db.Todos.Add(todo);
            await db.SaveChangesAsync();
            return Results.Created($"/api/v1/todos/{todo.Id}", todo);
        });

        group.MapPut("{id}", async (int id, Todo inputTodo, ApplicationDbContext db) =>
        {
            var todo = await db.Todos.FindAsync(id);
            if (todo is null) return Results.NotFound();

            todo.Title = inputTodo.Title;
            todo.DueBy = inputTodo.DueBy;
            todo.IsComplete = inputTodo.IsComplete;
            
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("{id}", async (int id, ApplicationDbContext db) =>
        {
            if (await db.Todos.FindAsync(id) is Todo todo)
            {
                db.Todos.Remove(todo);
                await db.SaveChangesAsync();
                return Results.Ok(todo);
            }
            return Results.NotFound();
        });
    }
}
