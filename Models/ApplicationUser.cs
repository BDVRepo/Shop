using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models;

public class ApplicationUser : IdentityUser
{
    [Display(Name = "Имя")]
    [StringLength(50)]
    public string? FirstName { get; set; }

    [Display(Name = "Фамилия")]
    [StringLength(50)]
    public string? LastName { get; set; }

    [Display(Name = "Пол")]
    [StringLength(20)]
    public string? Gender { get; set; } // Мужской, Женский, Не указывать

    [Display(Name = "Дата рождения")]
    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    [Display(Name = "Предпочитаемый размер")]
    [StringLength(10)]
    public string? PreferredSize { get; set; } // XS, S, M, L, XL, XXL

    [Display(Name = "Город")]
    [StringLength(100)]
    public string? City { get; set; }

    [Display(Name = "Дата регистрации")]
    public DateTime RegistrationDate { get; set; } = DateTime.Now;

    // Navigation properties
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

