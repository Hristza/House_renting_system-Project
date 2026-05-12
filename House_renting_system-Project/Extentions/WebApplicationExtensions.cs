using House_renting_system_Project.Data.Data;
using House_renting_system_Project.Data.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;
using static Azure.Core.HttpHeader;

namespace House_renting_system_Project.Extentions
{
    public static class WebApplicationExtensions
    {
        extension(WebApplication app)
        {
            public async Task SeedRoles()
            {
                using var scope = app.Services.CreateScope();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                foreach(var roleName in RoleNames.Names)
                {
                    var exists = await roleManager.RoleExistsAsync(roleName);
                    if (!exists) 
                    {
                        var role = new IdentityRole(roleName);
                        await roleManager.CreateAsync(role);
                    }

                    var defaltAgent = await userManager
                        .FindByIdAsync("13d2281e-1912-45f4-8aef-9c1288ac7fbc");
                    
                    if(defaltAgent is null)
                    {
                        var agent = new ApplicationUser
                        {
                            Id = "13d2281e-1912-45f4-8aef-9c1288ac7fbc",
                            UserName = UserCredentials.DefaultAgentUsername,
                            Email = UserCredentials.DefaultAgentEmail
                        };
                        await userManager.CreateAsync(agent, UserCredentials.DefaultAgentPassword);
                        await userManager.AddToRoleAsync(agent, RoleNames.Agent);
                    }
                }

            }

            public async Task SeedHouses()
            {
                using var scope = app.Services.CreateScope();

                var data = scope.ServiceProvider.GetRequiredService<HouseRentingDbContext>();
                if(!await data.Houses.AnyAsync())
                {
                    data.AddRange(
                        new House[]
            {
                new House() {
                    Id = 1,
                    Title = "Big House Marina",
                    Address = "North London, UK (near the border)",
                    Description = "A big house for your whole family. Don't miss to buy a house with three bedrooms.",
                    ImageUrl = "https://www.luxury-architecture.net/wp-content/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg",
                    PricePerMonth = 2100.00M,
                    CategoryId = 2,
                    AgentId = "13d2281e-1912-45f4-8aef-9c1288ac7fbc"
                },
                new House() {
                    Id = 2,
                    Title = "Family House Comfort",
                    Address = "Near the Sea Garden in Burgas, Bulgaria",
                    Description = "It has the best comfort you will ever ask for. With two bedrooms, it is great for your family.",
                    ImageUrl = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jp?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a&o=&hp=1",
                    PricePerMonth = 1200.00M,
                    CategoryId = 2,
                    AgentId = "13d2281e-1912-45f4-8aef-9c1288ac7fbc"
                    } 
                });
                    await data.SaveChangesAsync();
                }
            }
            
        }
    }
    public static class RoleNames
    {
        public static readonly string[] Names = [Agent, Client];
        public const string Agent = "Agent";
        public const string Client = "Client";
    }

    public static class UserCredentials
    {
        public const string DefaultAgentUsername = "agent-def";
        public const string DefaultAgentEmail = "agent@mail.com";
        public const string DefaultAgentPassword = "Qerty78_";

    }
}
