using Microsoft.EntityFrameworkCore;
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

        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Category? category = dbContext.Categories.Find(id);

            if (category is null)
            {
                return false;
            }

            dbContext.Categories.Remove(category);

            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Category> GetAsync(int id)
        {
            Category? category = dbContext.Categories.Find(id);

            if (category is null)
            {
                return new Category();
            }

            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            Category? existingCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

            if (existingCategory is not null)
            {
                existingCategory.Name = category.Name;
                dbContext.Categories.Update(existingCategory);

                await dbContext.SaveChangesAsync();

                return existingCategory;
            }

            return category;
        }
    }
}
