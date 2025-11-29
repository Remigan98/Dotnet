using FluentValidation;
using ToDos.MinimalAPI.Extensions;
using ToDos.MinimalAPI.ToDo.Validators;

namespace ToDos.MinimalAPI.ToDo;

public static class ToDoRequests
{
    public static WebApplication RegisterEndpoints(this WebApplication app)
    {
        app.MapGet("/todos", GetAll)
            .Produces<List<ToDo>>()
            .WithTags("To Dos");

        app.MapGet("/todos/{id:guid}", GetById)
            .Produces<ToDo>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags("To Dos");

        app.MapPost("/todos", Create)
            .Produces<ToDo>(StatusCodes.Status201Created)
            .Accepts<ToDo>("application/json")
            .WithTags("To Dos")
            .WithValidator<ToDo>();

        app.MapPut("/todos/{id:guid}", Update)
            .Produces(StatusCodes.Status202Accepted)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Accepts<ToDo>("application/json")
            .WithTags("To Dos")
            .WithValidator<ToDo>();

        app.MapDelete("/todos/{id:guid}", Delete)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags("To Dos")
            .ExcludeFromDescription();

        return app;
    }

    public static IResult GetAll(IToDoService service)
    {
        List<ToDo> toDos = service.GetAll();
        return Results.Ok(toDos);
    }

    public static IResult GetById(Guid id, IToDoService service)
    {
        ToDo? toDo = service.GetById(id);
        return toDo is not null ? Results.Ok(toDo) : Results.NotFound();
    }

    public static IResult Create(ToDo toDo, IToDoService service, IValidator<ToDo> validator)
    {
        service.Create(toDo);
        return Results.Created($"/todos/{toDo.Id}", toDo);
    }

    public static IResult Update(Guid id, ToDo toDo, IToDoService service, IValidator<ToDo> validator)
    {
        if (id != toDo.Id)
        {
            return Results.BadRequest();
        }

        bool updated = service.Update(toDo);
        return updated ? Results.Accepted() : Results.NotFound();
    }

    public static IResult Delete(Guid id, IToDoService service)
    {
        bool deleted = service.Delete(id);
        return deleted ? Results.NoContent() : Results.NotFound();
    } 
}
