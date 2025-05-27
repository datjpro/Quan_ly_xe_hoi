using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Car.Models
{
    public class ElectricCar
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Dung lượng pin (kWh)")]
        public double Battery { get; set; }
        
        [Required]
        [Display(Name = "Quãng đường (km)")]
        public int RangeKm { get; set; }
        
        [Required]
        [Display(Name = "Mẫu xe")]
        public string Model { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Hãng xe")]
        public int BranchId { get; set; }
        
        [Required]
        [Display(Name = "Giá bán")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        
        [Display(Name = "Hình ảnh")]
        public string? ImageUrl { get; set; }
        
        // Navigation property
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; } = null!;
    }
}
