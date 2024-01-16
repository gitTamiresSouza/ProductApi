using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Domain.Entities;

namespace ProductApi.Infrastructure.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context.Products.Any())
                {
                    return;
                }

                context.Products.AddRange(
                    new ProductEntity
                    {
                        Id = 1,
                        Name = "Notebook Dell Alienware",
                        Price = 15000,
                        Stock = 1
                    },
                    new ProductEntity
                    {
                        Id = 2,
                        Name = "Notebook Asus Rog",
                        Price = 20000,
                        Stock = 1
                    },
                    new ProductEntity
                    {
                        Id = 3,
                        Name = "Acer Nitro",
                        Price = 25000,
                        Stock = 1
                    },
                    new ProductEntity
                    {
                        Id = 4,
                        Name = "Razor Blade",
                        Price = 30000,
                        Stock = 1
                    },
                    new ProductEntity
                    {
                        Id = 5,
                        Name = "Lenovo Legion",
                        Price = 35000,
                        Stock = 1
                    });
                context.SaveChanges();
            }
        }
    }
}
