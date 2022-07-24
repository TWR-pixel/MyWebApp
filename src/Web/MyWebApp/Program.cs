using Microsoft.AspNetCore.Authentication.Cookies;
using MyWebApp.Domain.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/Admin/Login");
        options.AccessDeniedPath = new PathString("/Admin/Login");
    });

builder.Services.AddControllersWithViews();

var str = builder.Configuration.GetConnectionString("DbConnectionStr");

builder.Services.AddScoped(typeof(ImageService), x => new ImageService(str));
builder.Services.AddScoped(typeof(GroupService), x => new GroupService(str));
builder.Services.AddScoped(typeof(UserService), x => new UserService(str));

var app = builder.Build();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();