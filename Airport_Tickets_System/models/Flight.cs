namespace Airport_Tickets_System.models;

public class Flight
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public string DepartureCountry { get; set; }
    public string DestinationCountry { get; set; }
    public DateTime DepartureDate { get; set; }
    public string DepartureAirport { get; set; }
    public string ArrivalAirport { get; set; }
    public string Classes { get; set; }
}