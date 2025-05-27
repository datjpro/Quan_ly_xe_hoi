using System.ComponentModel.DataAnnotations;
using Car.Models;

namespace Car.ViewModels
{
    public class AddCarViewModel
    {
        public string CarType { get; set; } = "Electric"; // "Electric" or "Gas"
        public List<Branch> Branches { get; set; } = new List<Branch>();
        
        // Common properties
        [Required(ErrorMessage = "Vui lòng nhập tên mẫu xe")]
        [Display(Name = "Mẫu xe")]
        public string Model { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Vui lòng chọn hãng xe")]
        [Display(Name = "Hãng xe")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn hãng xe")]
        public int BranchId { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập giá xe")]
        [Display(Name = "Giá bán (USD)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá xe phải lớn hơn 0")]
        public decimal Price { get; set; }
        
        [Display(Name = "URL hình ảnh")]
        [Url(ErrorMessage = "Vui lòng nhập URL hợp lệ")]
        public string? ImageUrl { get; set; }
        
        // Electric car specific
        [Display(Name = "Dung lượng pin (kWh)")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Dung lượng pin phải lớn hơn 0")]
        public double? Battery { get; set; }
        
        [Display(Name = "Quãng đường (km)")]
        [Range(1, int.MaxValue, ErrorMessage = "Quãng đường phải lớn hơn 0")]
        public int? RangeKm { get; set; }
        
        // Gas car specific
        [Display(Name = "Mức tiêu thụ nhiên liệu (L/100km)")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Mức tiêu thụ phải lớn hơn 0")]
        public double? FuelEff { get; set; }
    }

    // ViewModel cho Index view - hiển thị cả xe điện và xe xăng
    public class CarViewModel
    {
        public List<ElectricCar> ElectricCars { get; set; } = new List<ElectricCar>();
        public List<GasCar> GasCars { get; set; } = new List<GasCar>();
        public List<Branch> Branches { get; set; } = new List<Branch>();
        public SearchViewModel Search { get; set; } = new SearchViewModel();
    }

    // ViewModel cho Search functionality
    public class SearchViewModel
    {
        [Display(Name = "Từ khóa tìm kiếm")]
        public string? SearchTerm { get; set; }
        
        [Display(Name = "Hãng xe")]
        public int? BranchId { get; set; }
        
        [Display(Name = "Loại xe")]
        public string? CarType { get; set; } // "Electric", "Gas", or null for all
        
        [Display(Name = "Giá từ")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        public decimal? MinPrice { get; set; }
        
        [Display(Name = "Giá đến")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        public decimal? MaxPrice { get; set; }
        
        // Results
        public List<ElectricCar> ElectricCars { get; set; } = new List<ElectricCar>();
        public List<GasCar> GasCars { get; set; } = new List<GasCar>();
        public List<Branch> Branches { get; set; } = new List<Branch>();
        
        // Search statistics
        public int TotalResults { get; set; }
        public bool HasSearched { get; set; }
    }

    // ViewModel riêng cho ElectricCar Details
    public class ElectricCarDetailsViewModel
    {
        public ElectricCar ElectricCar { get; set; } = new ElectricCar();
        public Branch Branch { get; set; } = new Branch();
    }

    // ViewModel riêng cho GasCar Details
    public class GasCarDetailsViewModel
    {
        public GasCar GasCar { get; set; } = new GasCar();
        public Branch Branch { get; set; } = new Branch();
    }
}
