using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Shop.Data;
using Shop.Models;
using System.Text;

namespace Shop.Services;

public class ExcelService
{
    private readonly ApplicationDbContext _context;

    public ExcelService(ApplicationDbContext context)
    {
        _context = context;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public async Task<byte[]> ExportProductsToExcelAsync()
    {
        var products = await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .ToListAsync();

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Товары");

        // Headers
        worksheet.Cells[1, 1].Value = "ID";
        worksheet.Cells[1, 2].Value = "Название";
        worksheet.Cells[1, 3].Value = "Описание";
        worksheet.Cells[1, 4].Value = "Цена";
        worksheet.Cells[1, 5].Value = "Бренд";
        worksheet.Cells[1, 6].Value = "Категория";
        worksheet.Cells[1, 7].Value = "Размеры";
        worksheet.Cells[1, 8].Value = "Цвет";
        worksheet.Cells[1, 9].Value = "Количество на складе";
        worksheet.Cells[1, 10].Value = "Доступен";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 10])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Data
        int row = 2;
        foreach (var product in products)
        {
            worksheet.Cells[row, 1].Value = product.Id;
            worksheet.Cells[row, 2].Value = product.Name;
            worksheet.Cells[row, 3].Value = product.Description;
            worksheet.Cells[row, 4].Value = product.Price;
            worksheet.Cells[row, 5].Value = product.Brand?.Name ?? "";
            worksheet.Cells[row, 6].Value = product.Category?.Name ?? "";
            worksheet.Cells[row, 7].Value = product.Sizes;
            worksheet.Cells[row, 8].Value = product.Color;
            worksheet.Cells[row, 9].Value = product.StockQuantity;
            worksheet.Cells[row, 10].Value = product.IsAvailable ? "Да" : "Нет";
            row++;
        }

        // Auto-fit columns
        worksheet.Cells.AutoFitColumns();

        return package.GetAsByteArray();
    }

    public async Task<(int success, int failed, List<string> errors)> ImportProductsFromExcelAsync(Stream stream)
    {
        int successCount = 0;
        int failedCount = 0;
        var errors = new List<string>();

        using var package = new ExcelPackage(stream);
        if (package.Workbook.Worksheets.Count == 0)
        {
            return (0, 0, new List<string> { "Файл не содержит рабочих листов" });
        }
        var worksheet = package.Workbook.Worksheets[0];

        var brands = await _context.Brands.ToListAsync();
        var categories = await _context.Categories.ToListAsync();

        int rowCount = worksheet.Dimension?.Rows ?? 0;

        for (int row = 2; row <= rowCount; row++)
        {
            try
            {
                var product = new Product
                {
                    Name = worksheet.Cells[row, 2].Text.Trim(),
                    Description = worksheet.Cells[row, 3].Text.Trim(),
                    Price = decimal.Parse(worksheet.Cells[row, 4].Text),
                    Sizes = worksheet.Cells[row, 7].Text.Trim(),
                    Color = worksheet.Cells[row, 8].Text.Trim(),
                    StockQuantity = int.Parse(worksheet.Cells[row, 9].Text),
                    IsAvailable = worksheet.Cells[row, 10].Text.ToLower() == "да",
                    CreatedAt = DateTime.Now
                };

                // Find brand
                var brandName = worksheet.Cells[row, 5].Text.Trim();
                var brand = brands.FirstOrDefault(b => b.Name.Equals(brandName, StringComparison.OrdinalIgnoreCase));
                if (brand == null)
                {
                    errors.Add($"Строка {row}: Бренд '{brandName}' не найден");
                    failedCount++;
                    continue;
                }
                product.BrandId = brand.Id;

                // Find category
                var categoryName = worksheet.Cells[row, 6].Text.Trim();
                var category = categories.FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
                if (category == null)
                {
                    errors.Add($"Строка {row}: Категория '{categoryName}' не найдена");
                    failedCount++;
                    continue;
                }
                product.CategoryId = category.Id;

                _context.Products.Add(product);
                successCount++;
            }
            catch (Exception ex)
            {
                errors.Add($"Строка {row}: Ошибка - {ex.Message}");
                failedCount++;
            }
        }

        if (successCount > 0)
        {
            await _context.SaveChangesAsync();
        }

        return (successCount, failedCount, errors);
    }

    public async Task<byte[]> ExportOrdersToExcelAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .ToListAsync();

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Заказы");

        // Headers
        worksheet.Cells[1, 1].Value = "Номер заказа";
        worksheet.Cells[1, 2].Value = "Дата";
        worksheet.Cells[1, 3].Value = "Пользователь";
        worksheet.Cells[1, 4].Value = "Статус";
        worksheet.Cells[1, 5].Value = "Сумма";
        worksheet.Cells[1, 6].Value = "Адрес доставки";

        // Style headers
        using (var range = worksheet.Cells[1, 1, 1, 6])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }

        // Data
        int row = 2;
        foreach (var order in orders)
        {
            worksheet.Cells[row, 1].Value = order.OrderNumber;
            worksheet.Cells[row, 2].Value = order.OrderDate.ToString("dd.MM.yyyy HH:mm");
            worksheet.Cells[row, 3].Value = order.User?.Email ?? "";
            worksheet.Cells[row, 4].Value = order.Status;
            worksheet.Cells[row, 5].Value = order.TotalAmount;
            worksheet.Cells[row, 6].Value = order.DeliveryAddress ?? "";
            row++;
        }

        worksheet.Cells.AutoFitColumns();
        return package.GetAsByteArray();
    }
}

