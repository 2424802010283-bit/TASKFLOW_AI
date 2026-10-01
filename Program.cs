using Microsoft.EntityFrameworkCore;

using TASKFLOW_AI.Data;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// 1. BẮT BUỘC THÊM ĐOẠN NÀY ĐỂ MỞ CỔNG KẾT NỐI SQL SERVER
builder.Services.AddDbContext<TaskFlowDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. CÁC CẤU HÌNH MẶC ĐỊNH CỦA HỆ THỐNG
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/Login";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
    });

// 3. DÒNG NÀY LÀ RANH GIỚI - MỌI CẤU HÌNH BUILDER PHẢI NẰM TRÊN DÒNG NÀY
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

app.Run();