<<<<<<< HEAD
<<<<<<< HEAD
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess.Data;
=======
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAcess.Data;
>>>>>>> ff020d72a70de9930dcff6a546e98ba02efb5e87
=======
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAcess.Data;
>>>>>>> ff020d72a70de9930dcff6a546e98ba02efb5e87
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
<<<<<<< HEAD
<<<<<<< HEAD
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
=======
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
>>>>>>> ff020d72a70de9930dcff6a546e98ba02efb5e87
=======
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
>>>>>>> ff020d72a70de9930dcff6a546e98ba02efb5e87

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
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
