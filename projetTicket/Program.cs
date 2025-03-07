using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using projetTicket.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<projetTicketContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("projetTicketContext") ?? throw new InvalidOperationException("Connection string 'projetTicketContext' not found.")));


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.IOTimeout = TimeSpan.FromDays(1);
    options.Cookie.Name = "CookieConnexion";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Path = "/";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
