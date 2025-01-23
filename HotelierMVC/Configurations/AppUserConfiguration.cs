using HotelierMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelierMVC.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(c => c.Name).IsRequired().HasColumnType("varchar(50)");
            builder.Property(c => c.Surname).IsRequired().HasColumnType("varchar(50)");

        }
    }
}
