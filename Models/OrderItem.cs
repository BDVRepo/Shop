using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models;

public class OrderItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Заказ")]
    public int OrderId { get; set; }

    [Required]
    [Display(Name = "Товар")]
    public int ProductId { get; set; }

    [Display(Name = "Количество")]
    [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть больше 0")]
    public int Quantity { get; set; } = 1;

    [Display(Name = "Цена за единицу")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Display(Name = "Размер")]
    [StringLength(20)]
    public string? Size { get; set; }

    // Navigation properties
    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }
}

