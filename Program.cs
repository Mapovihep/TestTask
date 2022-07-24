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

// Configure the HTTP request pipeline.
/*if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}*/

//app.UseHttpsRedirection();
//app.UseStaticFiles();

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.UseAuthorization();

//app.MapRazorPages();

app.Run("http://localhost:3000");
