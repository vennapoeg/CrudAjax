using System.ComponentModel.DataAnnotations;

namespace GrudAjax.Models
{
  public class Country
  {
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(3)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(75)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(75)] public string CurrencyName { get; set; } = string.Empty;

  }
}

