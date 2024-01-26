using System.Reflection.PortableExecutable;
using System.Net.Security;
using Ikigenda.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString =
     builder.Configuration.GetConnectionString("MariaDb")
    ?? throw new InvalidOperationException("Connection string 'MariaDb' not found.");
 builder.Services.AddDbContext<AgendaDbContext>(options =>
 {
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
 });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "RequireAdminOrStaff",
        policy => policy.RequireRole("Administrador", "Staff")
    );
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly=true;
        options.ExpireTimeSpan=TimeSpan.FromMinutes(60);
        // using Microsoft.AspNetCore.Http;
        options.LoginPath ="/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";

        //options.ReturnUrlParameter = "ReturnUrl";

        // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-3.1#>
        // options.EventsType = typeof(CustomCookieAuthenticationEvents);
    });

//CONFIGURAR SERVICIOS A UTILIZAR

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
