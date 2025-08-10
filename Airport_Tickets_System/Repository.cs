using Airport_Tickets_System.Database;
using Airport_Tickets_System.models;
using Airport_Tickets_System.states;

namespace Airport_Tickets_System;

public class Repository
{
    private readonly LoginDataBase _loginDataBase = new LoginDataBase();
    private readonly FlightsDatabase _flightsDatabase = new FlightsDatabase();
    private readonly BookingDatabase _bookingDatabase = new BookingDatabase();
    private readonly PassengerDatabase _passengerDatabase = new PassengerDatabase();

    public LoginResult ValidateUserCredentials(User user)
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
        string? departureCountry = null,
        string? destinationCountry = null,
        decimal? price = null,
        DateTime? departureDate = null,
        string? departureAirport = null,
        string? arrivalAirport = null,
        string? passengerName = null,
        string? bookingClass = null)
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

    public List<Flight> SearchFlights(
        string? departureCountry = null,
        string? destinationCountry = null,
        decimal? maxPrice = null,
        DateTime? departureDate = null,
        string? departureAirport = null,
        string? arrivalAirport = null,
        string? flightClass = null)
    {
        var flights = _flightsDatabase.GetAllFlights();

        var filtered = flights.Where(f =>
            (string.IsNullOrEmpty(departureCountry) ||
             f.DepartureCountry.Equals(departureCountry, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(destinationCountry) ||
             f.DestinationCountry.Equals(destinationCountry, StringComparison.OrdinalIgnoreCase)) &&
            (!maxPrice.HasValue || f.Price <= maxPrice.Value) &&
            (!departureDate.HasValue || f.DepartureDate.Date == departureDate.Value.Date) &&
            (string.IsNullOrEmpty(departureAirport) ||
             f.DepartureAirport.Equals(departureAirport, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(arrivalAirport) ||
             f.ArrivalAirport.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(flightClass) || f.Classes.Split(',',
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Any(c => c.Equals(flightClass, StringComparison.OrdinalIgnoreCase)))
        ).ToList();

        return filtered;
    }

    public bool BookFlight(string passengerUserName, int flightId, string flightClass)
    {
        var flight = _flightsDatabase.GetFlightById(flightId);
        if (flight == null)
            return false;

        var availableClasses =
            flight.Classes.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (!availableClasses.Any(c => c.Equals(flightClass, StringComparison.OrdinalIgnoreCase)))
            return false;

        // Create a new booking (assuming BookingDatabase has AddBooking method)
        var booking = new BookingDetail
        {
            BookingId = GenerateNewBookingId(),
            Passenger = _passengerDatabase.GetPassengerByUserName(passengerUserName),
            Flight = flight,
            Class = flightClass
        };

        return _bookingDatabase.AddBooking(booking);
    }

    public List<BookingDetail> GetBookingsByPassengerUserName(string userName)
    {
        var allBookings = _bookingDatabase.GetAllBookings();
        return allBookings.Where(b => b.Passenger != null && b.Passenger.Username == userName).ToList();
    }

    public bool CancelBooking(int bookingId, string passengerUserName)
    {
        var booking = _bookingDatabase.GetBookingById(bookingId);
        if (booking.Passenger == null || booking.Passenger.Username != passengerUserName)
            return false;

        return _bookingDatabase.RemoveBooking(bookingId);
    }

    public BookingDetail GetBookingById(int bookingId)
    {
        return _bookingDatabase.GetBookingById(bookingId);
    }

    public bool ModifyBooking(int bookingId, string passengerUserName, int newFlightId, string newClass)
    {
        var booking = _bookingDatabase.GetBookingById(bookingId);
        if (booking == null || booking.Passenger == null || booking.Passenger.Username != passengerUserName)
            return false;

        var flight = _flightsDatabase.GetFlightById(newFlightId);
        if (flight == null)
            return false;

        var availableClasses =
            flight.Classes.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (!availableClasses.Any(c => c.Equals(newClass, StringComparison.OrdinalIgnoreCase)))
            return false;

        booking.Flight = flight;
        booking.Class = newClass;

        return _bookingDatabase.UpdateBooking(booking);
    }


    private int GenerateNewBookingId()
    {
        var allBookings = _bookingDatabase.GetAllBookings();
        return (allBookings.Count == 0) ? 1 : allBookings.Max(b => b.BookingId) + 1;
    }
}