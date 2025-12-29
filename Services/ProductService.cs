using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Services;

public class ProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Product>> GetAvailableProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Where(p => p.IsAvailable && p.StockQuantity > 0)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Product>> SearchProductsAsync(string? searchTerm, int? brandId, int? categoryId)
    {
        var query = _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p => p.Name.Contains(searchTerm) || 
                                    (p.Description != null && p.Description.Contains(searchTerm)));
        }

        if (brandId.HasValue)
        {
            query = query.Where(p => p.BrandId == brandId.Value);
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
    }

    public async Task<bool> CreateProductAsync(Product product)
    {
        try
        {
            product.CreatedAt = DateTime.Now;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        try
        {
            product.UpdatedAt = DateTime.Now;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<Brand>> GetAllBrandsAsync()
    {
        return await _context.Brands.OrderBy(b => b.Name).ToListAsync();
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.OrderBy(c => c.Name).ToListAsync();
    }
}

