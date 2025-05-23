using BlogAPIWebApp.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<BlogAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllersWithViews();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    // Показувати детальну сторінку помилок у Dev середовищі
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseAuthorization();

app.MapRazorPages();

app.Run();