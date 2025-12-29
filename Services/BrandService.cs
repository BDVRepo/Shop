using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Services;

public class BrandService
{
    private readonly ApplicationDbContext _context;

    public BrandService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Brand>> GetAllBrandsAsync()
    {
        return await _context.Brands.OrderBy(b => b.Name).ToListAsync();
    }

    public async Task<Brand?> GetBrandByIdAsync(int id)
    {
        return await _context.Brands.FindAsync(id);
    }

    public async Task<bool> CreateBrandAsync(Brand brand)
    {
        try
        {
            brand.CreatedAt = DateTime.Now;
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateBrandAsync(Brand brand)
    {
        try
        {
            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteBrandAsync(int id)
    {
        try
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null) return false;

            // Check if brand is used in products
            var hasProducts = await _context.Products.AnyAsync(p => p.BrandId == id);
            if (hasProducts)
            {
                return false; // Cannot delete brand that has products
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}

