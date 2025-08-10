using Airport_Tickets_System.states;

namespace Airport_Tickets_System.models;

public class Booking
{
    public int Id { get; set; }
    public int PassengerId { get; set; }
    public int FlightId { get; set; }
    public string Class { get; set; }
}