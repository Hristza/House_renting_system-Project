using House_renting_system_Project.Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace House_renting_system_Project.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(SeedCategories());
        }

        private IEnumerable<Category> SeedCategories()
        {
            return new Category[]
            {
                //new Category()
                //{
                //    Id = 1,
                //    Name = "Cottage"
                //},

                //new Category()
                //{
                //    Id = 2,
                //    Name = "Single-Family"
                //},

                //new Category()
                //{
                //    Id = 3,
                //    Name = "Duplex"
                //}
            };
        }
    }
}
