using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Tasneem_Shop.DataAccess.Context;
using Tasneem_Shop.DataAccess.Implementation;
using Tasneem_Shop.Entities.Repositories;
using Tasneem_Shop.Entities.Servies;
using Utilities;

namespace Tasneem_Shop.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<IdentityUser,IdentityRole>(options => 
            {
                options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromHours(3);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;

            }).AddDefaultTokenProviders().AddDefaultUI().AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<ICartService, CartService>();

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddSingleton<IEmailSender, EmailSender>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{Area=Customer}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.MapControllerRoute(
                name: "Admin",
                pattern: "{Area=Admin}/{controller=Category}/{action=Index}/{id?}")  
                .WithStaticAssets();
            

            app.Run();
        }
    }
}
