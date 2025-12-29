using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models;

public class Brand
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Название бренда обязательно")]
    [Display(Name = "Название бренда")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Описание")]
    [StringLength(500)]
    public string? Description { get; set; }

    [Display(Name = "Страна происхождения")]
    [StringLength(50)]
    public string? Country { get; set; }

    [Display(Name = "Дата создания")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation properties
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

