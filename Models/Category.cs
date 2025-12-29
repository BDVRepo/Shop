using System.ComponentModel.DataAnnotations;

namespace Shop.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Название категории обязательно")]
    [Display(Name = "Название категории")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Описание")]
    [StringLength(500)]
    public string? Description { get; set; }

    [Display(Name = "Дата создания")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation properties
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

