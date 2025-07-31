namespace Airport_Tickets_System.models;

public record FlightDto(
    string Id,
    double Price,
    string DepartureCountry,
    string DestinationCountry,
    DateTime DepartureDate,
    string DepartureAirport,
    string ArrivalAirport,
    List<string> Classes
);