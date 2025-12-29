using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models;

public class Order
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Номер заказа")]
    [StringLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Пользователь")]
    public string UserId { get; set; } = string.Empty;

    [Display(Name = "Дата заказа")]
    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Display(Name = "Статус заказа")]
    [StringLength(50)]
    public string Status { get; set; } = "Новый"; // Новый, В обработке, Отправлен, Доставлен, Отменен

    [Display(Name = "Общая сумма")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Display(Name = "Адрес доставки")]
    [StringLength(500)]
    public string? DeliveryAddress { get; set; }

    [Display(Name = "Комментарий")]
    [StringLength(1000)]
    public string? Comment { get; set; }

    [Display(Name = "Телефон получателя")]
    [StringLength(20)]
    public string? RecipientPhone { get; set; }

    [Display(Name = "Имя получателя")]
    [StringLength(100)]
    public string? RecipientName { get; set; }

    // Navigation properties
    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

