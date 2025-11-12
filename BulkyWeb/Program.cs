using Bulky.DataAccess.Repository.IRepostory;
using Bulky.DataAccess;
using Microsoft.EntityFrameworkCore;
using Bulky.DataAccess.Repository;

var builder = WebApplication.CreateBuilder(args);

//if we need to register somthing for our application itd is done in program.cs

// Add services to the container.
builder.Services.AddControllersWithViews();

//mixed combination of both sqlserver and sqlite

var machineName = Environment.MachineName;
Console.WriteLine($"Running on machine: {machineName}");

if (machineName == "DESKTOP-M04RAKA")
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("MainDefaultConnection")));
}

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
   // pattern: "{controller=Home}/{action=Index}/{id?}");
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
