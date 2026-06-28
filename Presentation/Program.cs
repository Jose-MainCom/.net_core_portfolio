using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PortfolioDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("PortfolioDatabase"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<PortfolioDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/Login";
});

builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();

var app = builder.Build();

app.UseStatusCodePagesWithReExecute("/{0}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/500");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "404",
    pattern: "404",
    defaults: new
    {
        controller = "Error",
        action = "NotFoundPage"
    });

app.MapControllerRoute(
    name: "500",
    pattern: "500",
    defaults: new
    {
        controller = "Error",
        action = "ServerError"
    });

// Seed initial portfolio data
using var scope = app.Services.CreateScope();

var portfolioDbContext = scope.ServiceProvider
    .GetRequiredService<PortfolioDbContext>();

var userManager = scope.ServiceProvider
    .GetRequiredService<UserManager<ApplicationUser>>();

await PortfolioDbSeeder.SeedAsync(
    portfolioDbContext,
    userManager);

app.Run();