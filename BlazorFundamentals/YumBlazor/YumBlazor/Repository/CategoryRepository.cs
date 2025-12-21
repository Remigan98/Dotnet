using YumBlazor.Data;
using YumBlazor.Repository.Interfaces;

namespace YumBlazor.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Category Create(Category category)
        {
            dbContext.Categories.Add(category);
            dbContext.SaveChanges();

            return category;
        }

        public bool Delete(int id)
        {
            Category? obj = dbContext.Categories.Find(id);

            if (obj is null)
            {
                return false;
            }

            dbContext.Categories.Remove(obj);

            return dbContext.SaveChanges() > 0;
        }

        public Category Get(int id)
        {
            Category? category = dbContext.Categories.Find(id);

            if (category is null)
            {
                return new Category();
            }

            return category;
        }

        public IEnumerable<Category> GetAll()
        {
            return dbContext.Categories.ToList();
        }

        public Category Update(Category category)
        {
            Category? existingCategory = dbContext.Categories.FirstOrDefault(c => c.Id == category.Id);

            if (existingCategory is not null)
            {
                existingCategory.Name = category.Name;
                dbContext.Categories.Update(existingCategory);
                dbContext.SaveChanges();

                return existingCategory;
            }

            return category;
        }
    }
}
