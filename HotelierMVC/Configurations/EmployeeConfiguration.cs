using HotelierMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelierMVC.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(c => c.Name).IsRequired().HasColumnType("varchar(50)");
            builder.Property(c => c.Surname).IsRequired().HasColumnType("varchar(50)");

        }
    }
}
