using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.GlobalServices;
using school_major_project.Interfaces;
using school_major_project.Models;
using school_major_project.ModelServices;
using school_major_project.PaymentMethods.MoMo.Services;
using school_major_project.PaymentMethods.PayPal;
using school_major_project.PaymentMethods.VNPay.Services;
using school_major_project.Services;
var builder = WebApplication.CreateBuilder(args);

#region Builder Configuration


builder.Services.AddDbContext<ApplicationDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>()
       .AddEntityFrameworkStores<ApplicationDbContext>()
       .AddDefaultTokenProviders()
       .AddDefaultUI();

builder.Services.AddRazorPages();

// Add Chat Services


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
builder.Services.AddSingleton<IVnPayService, VnPayService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHostedService<ExpiredItemCleanupService>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/dang-nhap";
    options.LogoutPath = $"/dang-ky";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    options.ReturnUrlParameter = "returnUrl";
    options.Events.OnRedirectToAccessDenied = context =>
    {

        if (context.Request.Path.StartsWithSegments("/Admin", StringComparison.OrdinalIgnoreCase))
        {

            context.RedirectUri = "/Home/HandleError?code=404";
            if (context.Response.HasStarted)
            {

                return Task.CompletedTask;
            }

            context.Response.Redirect(context.RedirectUri);
        }
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Đường dẫn trang đăng nhập
        options.ReturnUrlParameter = "returnUrl"; // Tên tham số
    })
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Google:ClientSecret"];
    });
var emailConfig = builder.Configuration.GetSection("EmailSettings");
builder.Services.AddSingleton<IEmailService>(new EmailService(
    emailConfig["SmtpServer"],
    int.Parse(emailConfig["SmtpPort"]),
    emailConfig["FromEmail"],
    emailConfig["Username"],
    emailConfig["Password"]
));

builder.Services.AddTransient<IPayPalService, PayPalService>();
builder.Services.AddHttpClient<IMoMoService, MoMoService>();
builder.Services.AddScoped<IMoMoService, MoMoService>();
builder.Services.AddScoped<PrintingTicket>();
builder.Services.AddScoped<GoogleQuery>();

builder.Services.AddControllersWithViews();
#endregion




#region Configure Builder App
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Home/HandleError", "?code={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllers();

#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllerRoute(
        name: "Admin",
        pattern: "admin/{controller=AdminHome}/{action=Index}/{id?}",
        defaults: new { area = "Admin" });

    endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

});

app.Run();
#endregion