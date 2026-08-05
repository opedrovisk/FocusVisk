using FocusVisk.Application.Services;
using FocusVisk.Core.Interfaces;
using FocusVisk.Infrastructure.Data;
using FocusVisk.Infrastructure.Identity;
using FocusVisk.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(cfg => { }, typeof(FocusVisk.Application.AssemblyReference).Assembly);

builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddControllers();

builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

// app.MapHub<NotificationHub>("/hubs/notifications"); 

app.Run();