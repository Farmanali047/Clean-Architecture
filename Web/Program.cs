using Application.Services;
using Core.Interfaces;
using InfraStructure;
using Microsoft.EntityFrameworkCore;
using ProjectCarRental.Data;
using ProjectCarRental.Models;
using System.Security.Claims;
using Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<myAppUsers>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireClaim(ClaimTypes.Email,"Admin@drive.com"));
});
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IBookingform, BookingRepository>();
builder.Services.AddScoped<ICarRegisteration, CarRegisterationRepository>();
builder.Services.AddScoped(typeof(Core.Interfaces.IRepository<>),typeof(InfraStructure.GenericRepository<>));
builder.Services.AddScoped<Bookingform_service>();
builder.Services.AddScoped<CarRegisteration_Service>();
builder.Services.AddScoped(typeof(Generic_Service<>));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.MapHub<ChatHub>("/chatHub");

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
