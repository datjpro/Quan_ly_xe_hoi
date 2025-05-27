using System.ComponentModel.DataAnnotations;

namespace Car.Models
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Tên hãng")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Quốc gia")]
        public string Country { get; set; } = string.Empty;
        
        // Navigation properties
        public virtual ICollection<GasCar> GasCars { get; set; } = new List<GasCar>();
        public virtual ICollection<ElectricCar> ElectricCars { get; set; } = new List<ElectricCar>();
    }
}
