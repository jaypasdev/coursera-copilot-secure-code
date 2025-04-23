using Microsoft.EntityFrameworkCore;
using SafeVault.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// Add configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Configure in-memory database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("SafeVaultDb"));

var app = builder.Build();

app.MapGet("/", () => "Welcome to SafeVault!");
app.MapGet("/WebForm", async context =>
{
    await context.Response.WriteAsync(await File.ReadAllTextAsync("Views/WebForm.cshtml"));
});

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers(); // Enable attribute-based routing for controllers

app.Run();




