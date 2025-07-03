using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Builder Configuration


builder.Services.AddDbContext<ApplicationDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    // ... các option khác nếu cần
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
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
builder.Services.AddScoped<JwtTokenService>();

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