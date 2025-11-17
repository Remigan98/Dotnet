
namespace ToDos.MinimalAPI.ToDo
{
    public interface IToDoService
    {
        void Create(ToDo toDo);
        bool Delete(Guid id);
        List<ToDo> GetAll();
        ToDo? GetById(Guid id);
        bool Update(ToDo toDo);
    }
}