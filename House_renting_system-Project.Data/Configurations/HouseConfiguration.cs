using House_renting_system_Project.Data.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace House_renting_system_Project.Data.Configurations
{
    public class HouseConfiguration : IEntityTypeConfiguration<House>
    {
        public void Configure(EntityTypeBuilder<House> builder)
        {

            builder
                .HasOne(h => h.Category)
                .WithMany(c => c.Houses)
                .HasForeignKey(h => h.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(h => h.Agent)
                .WithMany(a => a.ManagedHouses)
                .HasForeignKey(h => h.AgentId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasData(SeedHouses());


        }

        private IEnumerable<House> SeedHouses()
        {
            return new House[]
            {
                //new House() {
                //    Id = 1,
                //    Title = "Big House Marina",
                //    Address = "North London, UK (near the border)",
                //    Description = "A big house for your whole family. Don't miss to buy a house with three bedrooms.",
                //    ImageUrl = "https://www.luxury-architecture.net/wp-content/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg",
                //    PricePerMonth = 2100.00M,
                //    CategoryId = 2,
                //    AgentId = 1
                //},
                //new House() {
                //    Id = 2,
                //    Title = "Family House Comfort",
                //    Address = "Near the Sea Garden in Burgas, Bulgaria",
                //    Description = "It has the best comfort you will ever ask for. With two bedrooms, it is great for your family.",
                //    ImageUrl = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jp?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a&o=&hp=1",
                //    PricePerMonth = 1200.00M,
                //    CategoryId = 2,
                //    AgentId = 1
                //}
            };
        }
    }
}
