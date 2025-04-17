var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
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




