using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Web_Learning.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


// Add authentication services (Cookie Authentication in this case)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";  // Redirect to the login page if not authenticated
        options.LogoutPath = "/Logout"; // Redirect to the logout page
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10); // Cookie expiration time
    });

// Add your database context or any other services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();  // Add this line 
app.UseRouting();

// Add authentication and authorization middleware
app.UseAuthentication();  // Add this line
app.UseAuthorization();

app.MapRazorPages();

app.Run();
