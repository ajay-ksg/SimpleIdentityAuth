using Microsoft.EntityFrameworkCore;
using SimpleAuthenticationAuthorization.Models.Entities;

namespace SimpleAuthenticationAuthorization.Database.Repository;

public class ProductRepository(AuthDbContext dbContext)
{
    public async Task<List<Product>> GetAll()
    {
        return await dbContext.Products.ToListAsync();
    }
    
    public async Task<Product> GetById(int id)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (product is null)
        {
            throw new InvalidOperationException("Product not found");
        }

        return product;
    }
    
    public async Task<Product> Create(Product product)
    {
        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();
        return product;
    }
}