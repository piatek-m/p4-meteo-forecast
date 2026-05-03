using System.Runtime.CompilerServices;
using MeteoForecast.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeteoForecast.Data.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(85); // Longest place name is 'Taumata­whakatangihanga­koauau­o­tamatea­turi­pukaka­piki­maunga­horo­nuku­pokai­whenua­ki­tana­tahu'

        builder.Property(c => c.Country)
            .IsRequired()
            .HasMaxLength(60);

        builder.Property(c => c.Latitude)
            .IsRequired();

        builder.Property(c => c.Longitude)
            .IsRequired();

        builder.Property(c => c.IsFavourite)
            .IsRequired()
            .HasDefaultValue(false);
    }
}