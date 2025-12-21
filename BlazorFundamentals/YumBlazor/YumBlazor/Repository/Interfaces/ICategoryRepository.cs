using YumBlazor.Data;

namespace YumBlazor.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        public Category Create(Category category);
        public Category Update(Category category);
        public bool Delete(int id);
        public Category Get(int id);
        public IEnumerable<Category> GetAll();
    }
}
