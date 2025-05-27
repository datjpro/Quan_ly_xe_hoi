using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Car.Data;
using Car.Models;
using Car.ViewModels;

namespace Car.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var viewModel = new CarViewModel
            {
                ElectricCars = await _context.ElectricCars
                    .Include(e => e.Branch)
                    .ToListAsync(),
                GasCars = await _context.GasCars
                    .Include(g => g.Branch)
                    .ToListAsync(),
                Branches = await _context.Branches.ToListAsync()
            };

            return View(viewModel);
        }

        // GET: Cars/Create
        public async Task<IActionResult> Create()
        {
            var branches = await _context.Branches.ToListAsync();
            
            // Thêm log để debug
            Console.WriteLine($"Số lượng branches: {branches.Count}");
            
            var model = new AddCarViewModel
            {
                Branches = branches
            };
            return View(model);
        }        // POST: Cars/CreateElectric
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateElectric(AddCarViewModel viewModel) // Đổi từ 'model' thành 'viewModel'
        {
            try
            {
                // Debug logs
                Console.WriteLine($"Model received: BranchId={viewModel.BranchId}, Model={viewModel.Model}");
                Console.WriteLine($"Battery={viewModel.Battery}, RangeKm={viewModel.RangeKm}, Price={viewModel.Price}");
                
                // Reset branches for validation errors
                viewModel.Branches = await _context.Branches.ToListAsync();
                
                // Validate required fields explicitly
                if (string.IsNullOrWhiteSpace(viewModel.Model))
                {
                    ModelState.AddModelError("Model", "Vui lòng nhập tên mẫu xe.");
                }
                
                if (viewModel.BranchId <= 0)
                {
                    ModelState.AddModelError("BranchId", "Vui lòng chọn hãng xe.");
                }
                else
                {
                    var branchExists = await _context.Branches.AnyAsync(b => b.Id == viewModel.BranchId);
                    if (!branchExists)
                    {
                        ModelState.AddModelError("BranchId", "Hãng xe được chọn không tồn tại.");
                    }
                }
                
                if (viewModel.Price <= 0)
                {
                    ModelState.AddModelError("Price", "Vui lòng nhập giá xe hợp lệ.");
                }
                
                if (!viewModel.Battery.HasValue || viewModel.Battery <= 0)
                {
                    ModelState.AddModelError("Battery", "Vui lòng nhập dung lượng pin hợp lệ.");
                }
                
                if (!viewModel.RangeKm.HasValue || viewModel.RangeKm <= 0)
                {
                    ModelState.AddModelError("RangeKm", "Vui lòng nhập quãng đường hợp lệ.");
                }
                
                // Debug model state
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("Model state is invalid:");
                    foreach (var error in ModelState)
                    {
                        Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                }
                
                if (ModelState.IsValid)
                {
                    var electricCar = new ElectricCar
                    {
                        Model = viewModel.Model.Trim(),
                        BranchId = viewModel.BranchId,
                        Price = viewModel.Price,
                        ImageUrl = string.IsNullOrWhiteSpace(viewModel.ImageUrl) ? null : viewModel.ImageUrl.Trim(),
                        Battery = viewModel.Battery ?? 0,
                        RangeKm = viewModel.RangeKm ?? 0
                    };
                    
                    _context.ElectricCars.Add(electricCar);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Thêm xe điện thành công!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", "Có lỗi xảy ra khi lưu dữ liệu: " + ex.Message);
                viewModel.Branches = await _context.Branches.ToListAsync();
            }
            
            viewModel.CarType = "Electric";
            return View("Create", viewModel);
        }        // POST: Cars/CreateGas
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGas(AddCarViewModel viewModel) // Đổi từ 'model' thành 'viewModel'
        {
            try
            {
                // Debug logs
                Console.WriteLine($"Gas Model received: BranchId={viewModel.BranchId}, Model={viewModel.Model}");
                Console.WriteLine($"FuelEff={viewModel.FuelEff}, Price={viewModel.Price}");
                
                // Reset branches for validation errors
                viewModel.Branches = await _context.Branches.ToListAsync();
                
                // Validate required fields explicitly
                if (string.IsNullOrWhiteSpace(viewModel.Model))
                {
                    ModelState.AddModelError("Model", "Vui lòng nhập tên mẫu xe.");
                }
                
                if (viewModel.BranchId <= 0)
                {
                    ModelState.AddModelError("BranchId", "Vui lòng chọn hãng xe.");
                }
                else
                {
                    var branchExists = await _context.Branches.AnyAsync(b => b.Id == viewModel.BranchId);
                    if (!branchExists)
                    {
                        ModelState.AddModelError("BranchId", "Hãng xe được chọn không tồn tại.");
                    }
                }
                
                if (viewModel.Price <= 0)
                {
                    ModelState.AddModelError("Price", "Vui lòng nhập giá xe hợp lệ.");
                }
                
                if (!viewModel.FuelEff.HasValue || viewModel.FuelEff <= 0)
                {
                    ModelState.AddModelError("FuelEff", "Vui lòng nhập mức tiêu thụ nhiên liệu hợp lệ.");
                }
                
                if (ModelState.IsValid)
                {
                    var gasCar = new GasCar
                    {
                        Model = viewModel.Model.Trim(),
                        BranchId = viewModel.BranchId,
                        Price = viewModel.Price,
                        ImageUrl = string.IsNullOrWhiteSpace(viewModel.ImageUrl) ? null : viewModel.ImageUrl.Trim(),
                        FuelEff = viewModel.FuelEff ?? 0
                    };
                    
                    _context.GasCars.Add(gasCar);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Thêm xe xăng thành công!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                ModelState.AddModelError("", "Có lỗi xảy ra khi lưu dữ liệu: " + ex.Message);
                viewModel.Branches = await _context.Branches.ToListAsync();
            }
            
            viewModel.CarType = "Gas";
            return View("Create", viewModel);
        }

        // GET: Cars/Search
        public async Task<IActionResult> Search()
        {
            var model = new SearchViewModel
            {
                Branches = await _context.Branches.ToListAsync()
            };
            return View(model);
        }

        // POST: Cars/Search
        [HttpPost]
        public async Task<IActionResult> Search(SearchViewModel model)
        {
            model.Branches = await _context.Branches.ToListAsync();
    
            if (model.HasSearched)
            {
                var electricQuery = _context.ElectricCars.Include(e => e.Branch).AsQueryable();
                var gasQuery = _context.GasCars.Include(g => g.Branch).AsQueryable();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(model.SearchTerm))
                {
                    electricQuery = electricQuery.Where(e => e.Model.Contains(model.SearchTerm));
                    gasQuery = gasQuery.Where(g => g.Model.Contains(model.SearchTerm));
                }

                if (model.BranchId.HasValue)
                {
                    electricQuery = electricQuery.Where(e => e.BranchId == model.BranchId);
                    gasQuery = gasQuery.Where(g => g.BranchId == model.BranchId);
                }

                if (model.MinPrice.HasValue)
                {
                    electricQuery = electricQuery.Where(e => e.Price >= model.MinPrice);
                    gasQuery = gasQuery.Where(g => g.Price >= model.MinPrice);
                }

                if (model.MaxPrice.HasValue)
                {
                    electricQuery = electricQuery.Where(e => e.Price <= model.MaxPrice);
                    gasQuery = gasQuery.Where(g => g.Price <= model.MaxPrice);
                }

                // Filter by car type
                if (model.CarType == "Electric")
                {
                    model.ElectricCars = await electricQuery.ToListAsync();
                    model.GasCars = new List<GasCar>();
                }
                else if (model.CarType == "Gas")
                {
                    model.ElectricCars = new List<ElectricCar>();
                    model.GasCars = await gasQuery.ToListAsync();
                }
                else
                {
                    model.ElectricCars = await electricQuery.ToListAsync();
                    model.GasCars = await gasQuery.ToListAsync();
                }

                model.TotalResults = model.ElectricCars.Count + model.GasCars.Count;
            }

            return View(model);
        }
    }
}
