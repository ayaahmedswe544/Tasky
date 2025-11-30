using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tasky.Models;
using Tasky.Repositories.IRepos;
using Tasky.Repositories.Repos;
using Tasky.Services.IServs;
using Tasky.Services.Servs;

namespace Tasky
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

         builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddScoped<Repositories.IRepos.IAccountRepo, Repositories.Repos.AccountRepo>();
            builder.Services.AddScoped<Services.IServs.IAccountServs, Services.Servs.AccountServs>();

            builder.Services.AddScoped<ITaskRepo,TaskRepo>();
            builder.Services.AddScoped<ITaskServs,TaskServs>();
            builder.Services.AddScoped<ICatRepo, CatRepo>();
            builder.Services.AddScoped<ICatServs, CatServs>();

           

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
                pattern: "{controller=Welcome}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
