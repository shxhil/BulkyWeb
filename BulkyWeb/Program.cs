using Bulky.DataAccess.Repository.IRepostory;
using Bulky.DataAccess;
using Microsoft.EntityFrameworkCore;
using Bulky.DataAccess.Repository;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

//if we need to register somthing for our application itd is done in program.cs after builder

// Add services to the container.
builder.Services.AddControllersWithViews();

//mixed combination of both sqlserver and sqlite

var machineName = Environment.MachineName;
bool useSqlite = machineName == "DESKTOP-M04RAKA";
Console.WriteLine($"Running on machine: {machineName}");

if (useSqlite)
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("MainDefaultConnection")));
}

builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddRazorPages();

var app = builder.Build();


// 2. 📌 Place DB initialization RIGHT HERE
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (useSqlite)
    {
        db.Database.EnsureCreated();   // Auto-create tables (SQLite)
    }
    else
    {
        db.Database.Migrate();         // Apply EF migrations (SQL Server)
    }
}

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
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
   // pattern: "{controller=Home}/{action=Index}/{id?}");
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
