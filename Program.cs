using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.GlobalServices;
using school_major_project.Services;
using school_major_project.Interfaces;
using school_major_project.ModelServices;
using Microsoft.AspNetCore.Identity;
using school_major_project.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
 
builder.Services.AddDbContext<ApplicationDbContext> (option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>()
       .AddEntityFrameworkStores<ApplicationDbContext>()
       .AddDefaultTokenProviders()
       .AddDefaultUI();

builder.Services.AddRazorPages();

builder.Services.AddScoped<IFilm, FilmService>();
builder.Services.AddScoped<ICategory, CategoryService>();
builder.Services.AddScoped<ISeatType, SeatTypeSerivce>();
builder.Services.AddScoped<IReceipt, ReceiptService>();
builder.Services.AddScoped<ICinema, CinemaService>();
builder.Services.AddScoped<IRoom, RoomService>();
builder.Services.AddScoped<IReceiptDetails,ReceiptDetailService>();
builder.Services.AddScoped<ISchedule, ScheduleService>();
builder.Services.AddScoped<ISeat, SeatService>();
builder.Services.AddScoped<ICountry, CountryService>();
builder.Services.AddScoped<ISeat, SeatService>();
builder.Services.AddScoped<IBlog, BlogService>();
builder.Services.AddScoped<IComment, CommentService>();
builder.Services.AddScoped<IRating, RatingService>();
builder.Services.AddScoped<IFood, FoodService>();
builder.Services.AddScoped<IPromotion, PromotionService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHostedService<ExpiredItemCleanupService>();

builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = $"/Identity/Pages/Account/Login";
    option.LogoutPath = $"/Identity/Pages/Account/Logout";
    option.AccessDeniedPath = $"/Identity/Pages/Account/AccessDenied";
});




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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.Run();
