namespace MeteoForecast.Models;

public class City
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Country { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsFavourite { get; set; }
    public DateTime AddedAt { get; set; }
}