using Microsoft.EntityFrameworkCore;
using YumBlazor.Data;
using YumBlazor.Repository.Interfaces;

namespace YumBlazor.Repository
{
    public class ProductRepository : IProductRepository
    {
        readonly ApplicationDbContext dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Product? product = dbContext.Products.Find(id);

            if (product is null)
            {
                return false;
            }

            dbContext.Products.Remove(product);

            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Product> GetAsync(int id)
        {
            Product? product = dbContext.Products.Find(id);

            if (product is null)
            {
                return new Product();
            }

            return product;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await dbContext.Products.ToListAsync();
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            Product? existingProduct = await dbContext.Products.FirstOrDefaultAsync(c => c.Id == product.Id);

            if (existingProduct is not null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.Tag = product.Tag;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = product.ImageUrl;
                dbContext.Products.Update(existingProduct);

                await dbContext.SaveChangesAsync();

                return existingProduct;
            }

            return product;
        }
    }
}
