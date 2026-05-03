using MeteoForecast.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeteoForecast.Data.Configurations;

public class WeatherCacheConfiguration : IEntityTypeConfiguration<WeatherCache>
{
    public void Configure(EntityTypeBuilder<WeatherCache> builder)
    {
        builder.Property(wc => wc.Date)
            .IsRequired();

        builder.Property(wc => wc.FetchedAt)
            .IsRequired();

        builder.HasIndex(wc => new { wc.CityId, wc.Date })
            .IsUnique();

        // City -<1:N>- WeatherCache
        builder.HasOne(wc => wc.City)
            .WithMany()
            .HasForeignKey(wc => wc.CityId)
            .OnDelete(DeleteBehavior.Cascade);

        // WeatherCache -<1:N>- HourlyData
        builder.HasMany(wc => wc.HourlyData)
            .WithOne()
            .HasForeignKey(h => h.WeatherCacheId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}