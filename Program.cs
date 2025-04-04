﻿using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.GlobalServices;
using school_major_project.Services;
using school_major_project.Interfaces;
using school_major_project.ModelServices;
using Microsoft.AspNetCore.Identity;
using school_major_project.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>()
       .AddEntityFrameworkStores<ApplicationDbContext>()
       .AddDefaultTokenProviders()
       .AddDefaultUI();

builder.Services.AddRazorPages();

builder.Services.AddScoped<IFilmRepository, FilmService>();
builder.Services.AddScoped<ICategoryRepository, CategoryService>();
builder.Services.AddScoped<ISeatTypeRepository, SeatTypeSerivce>();
builder.Services.AddScoped<IReceiptRepository, ReceiptService>();
builder.Services.AddScoped<ICinemaRepository, CinemaService>();
builder.Services.AddScoped<IRoomRepository, RoomService>();
builder.Services.AddScoped<IReceiptDetailsRepository, ReceiptDetailService>();
builder.Services.AddScoped<IScheduleRepository, ScheduleService>();
builder.Services.AddScoped<ISeatRepository, SeatService>();
builder.Services.AddScoped<ICountryRepository, CountryService>();
builder.Services.AddScoped<ISeatRepository, SeatService>();
builder.Services.AddScoped<IBlogRepository, BlogService>();
builder.Services.AddScoped<ICommentRepository, CommentService>();
builder.Services.AddScoped<IRatingRepository, RatingService>();
builder.Services.AddScoped<IFoodRepository, FoodService>();
builder.Services.AddScoped<IPromotionRepository, PromotionService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHostedService<ExpiredItemCleanupService>();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddAreaPageRoute(
        areaName: "Identity",
        pageName: "/Account/Login",
        route: "dang-nhap"
    );
    options.Conventions.AddAreaPageRoute(
       areaName: "Identity",
       pageName: "/Account/Register",
       route: "dang-ky"
   );
});

builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = $"/dang-nhap";
    option.LogoutPath = $"/dang-ky";
    option.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    option.ReturnUrlParameter = "returnUrl"; // Tên tham số returnUrl
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Đường dẫn trang đăng nhập
        options.ReturnUrlParameter = "returnUrl"; // Tên tham số
    });
var emailConfig = builder.Configuration.GetSection("EmailSettings");
builder.Services.AddSingleton<IEmailService>(new EmailService(
    emailConfig["SmtpServer"],
    int.Parse(emailConfig["SmtpPort"]),
    emailConfig["FromEmail"],
    emailConfig["Username"],
    emailConfig["Password"]
));

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

#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllerRoute(
        name: "Admin",
        pattern: "admin/{controller=Home}/{action=Index}/{id?}",
        defaults: new { area = "Admin" });

    endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

});

app.Run();
