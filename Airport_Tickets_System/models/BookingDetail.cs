namespace Airport_Tickets_System.models;

public class BookingDetail
{
    public int BookingId { get; set; }
    public Passenger Passenger { get; set; }
    public Flight Flight { get; set; }
    public string Class { get; set; }

    public override string ToString()
    {
        return $"{BookingId} | {Passenger?.FullName ?? "N/A"} | " +
               $"{Flight?.DepartureCountry ?? "N/A"} -> {Flight?.DestinationCountry ?? "N/A"} | " +
               $"Class: {Class ?? "N/A"} | Price: {Flight?.Price.ToString("C") ?? "N/A"}";
    }
}