using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Название товара обязательно")]
    [Display(Name = "Название товара")]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Описание")]
    [StringLength(2000)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Цена обязательна")]
    [Display(Name = "Цена")]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, 999999.99, ErrorMessage = "Цена должна быть больше 0")]
    public decimal Price { get; set; }

    [Display(Name = "Изображение")]
    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [Display(Name = "Размеры (через запятую)")]
    [StringLength(100)]
    public string? Sizes { get; set; } // "S,M,L,XL" или "40,42,44"

    [Display(Name = "Цвет")]
    [StringLength(50)]
    public string? Color { get; set; }

    [Display(Name = "Количество на складе")]
    public int StockQuantity { get; set; } = 0;

    [Display(Name = "Доступен для заказа")]
    public bool IsAvailable { get; set; } = true;

    [Display(Name = "Дата создания")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Display(Name = "Дата обновления")]
    public DateTime? UpdatedAt { get; set; }

    // Foreign keys
    [Required(ErrorMessage = "Бренд обязателен")]
    [Display(Name = "Бренд")]
    public int BrandId { get; set; }

    [Required(ErrorMessage = "Категория обязательна")]
    [Display(Name = "Категория")]
    public int CategoryId { get; set; }

    // Navigation properties
    [ForeignKey("BrandId")]
    public virtual Brand? Brand { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

