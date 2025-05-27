using Microsoft.EntityFrameworkCore;
using Car.Data;
using Car.Models; // Thêm dòng này

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // Ensure database is created
    context.Database.EnsureCreated();
    
    // Seed branches if empty
    if (!context.Branches.Any())
    {
        var branches = new List<Branch>
        {
            new Branch { Name = "Toyota", Country = "Japan" },
            new Branch { Name = "Honda", Country = "Japan" },
            new Branch { Name = "Ford", Country = "USA" },
            new Branch { Name = "BMW", Country = "Germany" },
            new Branch { Name = "Mercedes-Benz", Country = "Germany" },
            new Branch { Name = "Tesla", Country = "USA" },
            new Branch { Name = "Hyundai", Country = "South Korea" },
            new Branch { Name = "Volkswagen", Country = "Germany" }
        };
        
        context.Branches.AddRange(branches);
        context.SaveChanges();
        
        Console.WriteLine("Seeded branches data successfully!");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
