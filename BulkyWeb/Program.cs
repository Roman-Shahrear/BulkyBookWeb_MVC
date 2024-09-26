using Microsoft.AspNetCore.Identity.UI.Services;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BulkyBook.Utility;
using Microsoft.Extensions.Options;
using Stripe;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//,
//        sqlServerOptionsAction: sqlOptions =>
//        {
//            sqlOptions.EnableRetryOnFailure(
//                maxRetryCount: 10,
//                maxRetryDelay: TimeSpan.FromSeconds(5),
//                errorNumbersToAdd: null);
//        }
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
//{
//    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
//    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
//        sqlOptions => sqlOptions.EnableRetryOnFailure());
//});



//Add services for Stripe
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

//For Email confirmed Account to use option goes to options
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

//Add services for application or user cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

//configuratuin for Razor Pages
builder.Services.AddRazorPages();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

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

//Set stripe
StripeConfiguration.ApiKey = builder.Configuration.GetValue<string>("Stripe:SecretKey");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// for map or routing Razor Pages
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");


app.Run();
