using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoLibrary.EFModals;

public class Product

{
    [Key]
    public int id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(6,2)")]
    public decimal Price { get; set; }
}
