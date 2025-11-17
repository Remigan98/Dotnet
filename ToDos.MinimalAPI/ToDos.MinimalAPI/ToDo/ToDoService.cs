namespace ToDos.MinimalAPI.ToDo
{
    public class ToDoService : IToDoService
    {
        private readonly Dictionary<Guid, ToDo> toDos = new();

        public ToDoService()
        {
            ToDo sampleToDo = new ToDo { Value = "Sample ToDo Item" };
            this.toDos[sampleToDo.Id] = sampleToDo;
        }

        public ToDo? GetById(Guid id)
        {
            return this.toDos.TryGetValue(id, out var toDo) ? toDo : null;
        }

        public List<ToDo> GetAll()
        {
            return this.toDos.Values.ToList();
        }

        public void Create(ToDo toDo)
        {
            if (toDo is null)
            {
                return;
            }

            this.toDos[toDo.Id] = toDo;
        }

        public bool Update(ToDo toDo)
        {
            if (toDo is null || !this.toDos.ContainsKey(toDo.Id))
            {
                return false;
            }
            this.toDos[toDo.Id] = toDo;
            return true;
        }

        public bool Delete(Guid id)
        {
            return this.toDos.Remove(id);
        }
    }
}
