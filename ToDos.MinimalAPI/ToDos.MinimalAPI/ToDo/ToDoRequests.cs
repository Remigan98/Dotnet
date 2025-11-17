namespace ToDos.MinimalAPI.ToDo;

public static class ToDoRequests
{
    public static WebApplication RegisterEndpoints(this WebApplication app)
    {
        app.MapGet("/todos", GetAll);
        app.MapGet("/todos/{id:guid}", GetById);
        app.MapPost("/todos", Create);
        app.MapPut("/todos/{id:guid}", Update);
        app.MapDelete("/todos/{id:guid}", Delete);

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

    public static IResult Create(ToDo toDo, IToDoService service)
    {
        service.Create(toDo);
        return Results.Created($"/todos/{toDo.Id}", toDo);
    }

    public static IResult Update(Guid id, ToDo toDo, IToDoService service)
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
