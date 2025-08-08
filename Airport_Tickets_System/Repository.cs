using Airport_Tickets_System.Database;
using Airport_Tickets_System.models;
using Airport_Tickets_System.states;

namespace Airport_Tickets_System;

public class Repository
{
    private readonly LoginDataBase _loginDataBase = new LoginDataBase();
    private readonly FlightsDatabase _flightsDatabase = new FlightsDatabase();
    private readonly BookingDatabase _bookingDatabase = new BookingDatabase();


    public LoginState ValidateUserCredentials(User user)
    {
        return _loginDataBase.ValidateUserCredentials(user);
    }

    public bool InsertFlightsData(string[] flights)
    {
        return _flightsDatabase.InsertFlightsData(flights);
    }

    public List<BookingDetail> GetAllBookings()
    {
        return _bookingDatabase.GetAllBookings();
    }

    public List<BookingDetail> FilterBookings(
        string departureCountry = null,
        string destinationCountry = null,
        decimal? price = null,
        DateTime? departureDate = null,
        string departureAirport = null,
        string arrivalAirport = null,
        string passengerName = null,
        string bookingClass = null)
    {
        return _bookingDatabase.FilterBookings(
            departureCountry, 
            destinationCountry, 
            price,
            departureDate, 
            departureAirport,
            arrivalAirport, 
            passengerName,
            bookingClass
        );
    }
}