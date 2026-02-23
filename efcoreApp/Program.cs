using Microsoft.EntityFrameworkCore;
using efcoreApp.Data;
using Microsoft.Extensions.DependencyModel.Resolution;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews(); 

builder.Services.AddDbContext<DataContext>(options =>
{
     var config = builder.Configuration; //Uygulama yapılandırma ayarlarını (config)alır.
    var connectionString = config.GetConnectionString("database"); //Bağlantı dizesini alır(connectionString)
    options.UseSqlite(connectionString);//SqlLite Veritabanı sağlayıcısını kullanır.
});
var app = builder.Build();  

if (!app.Environment.IsDevelopment()) 
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
