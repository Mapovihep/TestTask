using TestTask.Context;
using TestTask.DTO;
using TestTask.Models;
using TestTask.Repository;
using TestTask.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IRepository<Transaction>, TransactionRepository>();
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


static WebApplicationBuilder CreateHost(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllersWithViews();
    builder.Services.AddSingleton<IRepository<Transaction>, TransactionRepository>();

    builder.Services.AddSingleton<ITransactionService, TransactionService>();
    builder.Services.AddSingleton<ApplicationContext>();
    builder.Services.AddSingleton<ITransactionMapper, TransactionMapper>();
    return builder;
}