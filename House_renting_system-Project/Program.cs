using House_renting_system_Project.Data.Data;
using House_renting_system_Project.Data.Data.Entities;
using House_renting_system_Project.Extentions;
using House_renting_system_Project.Servises.Contracts;
using House_renting_system_Project.Servises.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace House_renting_system_Project
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<HouseRentingDbContext>(opt => opt.UseSqlServer(connectionString));
            
            

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.SignIn.RequireConfirmedEmail = false;
            }
            )
                .AddEntityFrameworkStores<HouseRentingDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddScoped<IHouseService, HouseService>();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.AccessDeniedPath = "/User/AccessDenied";
            });

            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<HouseRentingDbContext>();
                await db.Database.MigrateAsync();
            }
            await app.SeedRoles();
            await app.SeedHouses();
                //app.UseTimer();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/ServerError");
                app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.Use(async (context, next) =>
            {
                //incoming request
                var path = context.Request.Path;
                Console.WriteLine(path);
                await next();
                //outgoing respons
                var statusCode = context.Response.StatusCode;
                Console.WriteLine(statusCode);
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            //app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }        
    }
}
