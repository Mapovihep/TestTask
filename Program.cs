using TestTask.Context;
using TestTask.DTO;
using TestTask.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ITransactionService, TransactionService>();
builder.Services.AddSingleton<ApplicationContext>();
builder.Services.AddSingleton<ITransactionMapper, TransactionMapper>();
var app = builder.Build();
app.UseRouting();

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.UseAuthorization();

app.Run("http://localhost:3000");
