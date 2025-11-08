using LibraryManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Controller + View + SignalR
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

// swagger setting
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LibraryManagement API",
        Version = "v1",
        Description = "API for managing books, customers, and more"
    });
});

// database setting
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// login restrictions
builder.Services.AddIdentity<Customer, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// 3rd party login(Google + GitHub) - only configure if valid credentials are provided
var authBuilder = builder.Services.AddAuthentication();

var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
if (!string.IsNullOrEmpty(googleClientId) && 
    !string.IsNullOrEmpty(googleClientSecret) &&
    googleClientId != "YOUR_GOOGLE_CLIENT_ID" &&
    googleClientSecret != "YOUR_GOOGLE_CLIENT_SECRET")
{
    authBuilder.AddGoogle(options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;
    });
}

var githubClientId = builder.Configuration["Authentication:GitHub:ClientId"];
var githubClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"];
if (!string.IsNullOrEmpty(githubClientId) && 
    !string.IsNullOrEmpty(githubClientSecret) &&
    githubClientId != "YOUR_GITHUB_CLIENT_ID" &&
    githubClientSecret != "YOUR_GITHUB_CLIENT_SECRET")
{
    authBuilder.AddGitHub(options =>
    {
        options.ClientId = githubClientId;
        options.ClientSecret = githubClientSecret;
    });
}

var app = builder.Build();

// error handling
app.UseExceptionHandler("/Error/Exception"); //no if conditon so that all environment will work
app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");

// Swagger (dev environment only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LibraryManagement v1");
        c.RoutePrefix = "swagger"; //url swagger
    });
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// MVC routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// SignalR routing
app.MapHub<ChatHub>("/chathub");

app.Run();