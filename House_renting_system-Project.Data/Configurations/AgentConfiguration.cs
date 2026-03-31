using House_renting_system_Project.Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace House_renting_system_Project.Data.Configurations
{
    public class AgentConfiguration : IEntityTypeConfiguration<Agent>
    {
        public void Configure(EntityTypeBuilder<Agent> builder)
        {
            builder.HasData(SeedAgent());
        }
        private IEnumerable<Agent> SeedAgent()
        {
            return new Agent[]
            {
            //    new Agent()
            //    {
            //    Id = 1,
            //    PhoneNumber = "+359888888888",
            //    UserId = "ebwfdwefw"
            //    },

            //new Agent()
            //    {
            //        Id = 2,
            //        PhoneNumber = "+359988888888",
            //        UserId = "ewuhrwhfewfver"
            //    }
            };
        }
    }
}
