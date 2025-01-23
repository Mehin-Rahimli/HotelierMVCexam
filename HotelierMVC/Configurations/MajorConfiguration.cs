using HotelierMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelierMVC.Configurations
{
    public class MajorConfiguration : IEntityTypeConfiguration<Major>
    {
        public void Configure(EntityTypeBuilder<Major> builder)
        {
            builder.Property(c => c.Name).IsRequired().HasColumnType("varchar(50)");
            builder.HasIndex(c => c.Name).IsUnique();

        }
    }
}
