using Microsoft.AspNetCore.Identity;
using Shop.Models;

namespace Shop.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Create roles
        string[] roleNames = { "Admin", "Manager", "Customer" };
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Create default admin user
        var adminEmail = "admin@shop.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "Администратор",
                LastName = "Системы",
                Gender = "Не указывать",
                City = "Москва",
                RegistrationDate = DateTime.Now
            };
            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // Seed brands if empty
        if (!context.Brands.Any())
        {
            var brands = new List<Brand>
            {
                new Brand { Name = "Nike", Description = "Американский бренд спортивной одежды", Country = "США", CreatedAt = DateTime.Now },
                new Brand { Name = "Adidas", Description = "Немецкий бренд спортивной одежды", Country = "Германия", CreatedAt = DateTime.Now },
                new Brand { Name = "Gucci", Description = "Итальянский люксовый бренд", Country = "Италия", CreatedAt = DateTime.Now },
                new Brand { Name = "Zara", Description = "Испанский бренд модной одежды", Country = "Испания", CreatedAt = DateTime.Now },
                new Brand { Name = "H&M", Description = "Шведский бренд масс-маркета", Country = "Швеция", CreatedAt = DateTime.Now }
            };
            context.Brands.AddRange(brands);
            await context.SaveChangesAsync();
        }

        // Seed categories if empty
        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new Category { Name = "Футболки", Description = "Мужские и женские футболки", CreatedAt = DateTime.Now },
                new Category { Name = "Джинсы", Description = "Классические и современные джинсы", CreatedAt = DateTime.Now },
                new Category { Name = "Куртки", Description = "Ветровки, пуховики, демисезонные куртки", CreatedAt = DateTime.Now },
                new Category { Name = "Обувь", Description = "Кроссовки, ботинки, туфли", CreatedAt = DateTime.Now },
                new Category { Name = "Аксессуары", Description = "Сумки, ремни, головные уборы", CreatedAt = DateTime.Now }
            };
            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
        }

        // Seed products if empty
        if (!context.Products.Any())
        {
            var brands = context.Brands.ToList();
            var categories = context.Categories.ToList();

            if (brands.Any() && categories.Any())
            {
                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "Nike Air Max 90",
                        Description = "Классические кроссовки Nike с технологией Air Max",
                        Price = 8999,
                        ImageUrl = "/images/products/nike-airmax.jpg",
                        Sizes = "40,41,42,43,44,45",
                        Color = "Черный/Белый",
                        StockQuantity = 15,
                        IsAvailable = true,
                        BrandId = brands.First(b => b.Name == "Nike").Id,
                        CategoryId = categories.First(c => c.Name == "Обувь").Id,
                        CreatedAt = DateTime.Now
                    },
                    new Product
                    {
                        Name = "Adidas Originals T-Shirt",
                        Description = "Классическая футболка с логотипом Adidas",
                        Price = 2499,
                        ImageUrl = "/images/products/adidas-tshirt.jpg",
                        Sizes = "S,M,L,XL",
                        Color = "Белый",
                        StockQuantity = 30,
                        IsAvailable = true,
                        BrandId = brands.First(b => b.Name == "Adidas").Id,
                        CategoryId = categories.First(c => c.Name == "Футболки").Id,
                        CreatedAt = DateTime.Now
                    },
                    new Product
                    {
                        Name = "Zara Джинсы классические",
                        Description = "Классические синие джинсы прямого кроя",
                        Price = 3499,
                        ImageUrl = "/images/products/zara-jeans.jpg",
                        Sizes = "28,30,32,34,36",
                        Color = "Синий",
                        StockQuantity = 20,
                        IsAvailable = true,
                        BrandId = brands.First(b => b.Name == "Zara").Id,
                        CategoryId = categories.First(c => c.Name == "Джинсы").Id,
                        CreatedAt = DateTime.Now
                    }
                };
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
}

