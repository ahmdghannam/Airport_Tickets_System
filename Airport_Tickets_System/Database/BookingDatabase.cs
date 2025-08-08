using Airport_Tickets_System.models;
using Airport_Tickets_System.Utils;

namespace Airport_Tickets_System.Database;

public class BookingDatabase
{
    private readonly string[] _flightLines;
    private readonly string[] _passengerLines;
    private readonly string[] _bookingLines;

    public BookingDatabase()
    {
        string baseDir = AppContext.BaseDirectory;
        string projectDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\.."));
        string filesDir = Path.Combine(projectDir, Consts.Files.Package);

        _flightLines = File.ReadAllLines(Path.Combine(filesDir, Consts.Files.Flights));
        _passengerLines = File.ReadAllLines(Path.Combine(filesDir, Consts.Files.Passengers));
        _bookingLines = File.ReadAllLines(Path.Combine(filesDir, Consts.Files.Bookings));
    }

    public List<BookingDetail> GetAllBookings()
    {
        var flights = _flightLines.Skip(1).Select(line =>
        {
            var parts = line.Split(',');
            return new Flight
            {
                Id = int.Parse(parts[0]),
                Price = decimal.Parse(parts[1]),
                DepartureCountry = parts[2],
                DestinationCountry = parts[3],
                DepartureDate = DateTime.Parse(parts[4]),
                DepartureAirport = parts[5],
                ArrivalAirport = parts[6],
                Classes = parts[7]
            };
        }).ToList();

        var passengers = _passengerLines.Skip(1).Select(line =>
        {
            var parts = line.Split(',');
            return new Passenger
            {
                Id = int.Parse(parts[0]),
                Username = parts[1],
                FullName = parts[2],
                PhoneNumber = parts[3]
            };
        }).ToList();

        var bookings = _bookingLines.Skip(1).Select(line =>
        {
            var parts = line.Split(',');
            return new Booking
            {
                Id = int.Parse(parts[0]),
                PassengerId = int.Parse(parts[1]),
                FlightId = int.Parse(parts[2]),
                Class = parts[3]
            };
        }).ToList();

        var joinedData =
            from b in bookings
            join p in passengers on b.PassengerId equals p.Id
            join f in flights on b.FlightId equals f.Id
            select new BookingDetail
            {
                BookingId = b.Id,
                Passenger = p,
                Flight = f,
                Class = b.Class
            };

        return joinedData.ToList();
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
        var allBookings = GetAllBookings().AsQueryable();

        if (!string.IsNullOrEmpty(departureCountry))
            allBookings = allBookings.Where(b => b.Flight.DepartureCountry == departureCountry);

        if (!string.IsNullOrEmpty(destinationCountry))
            allBookings = allBookings.Where(b => b.Flight.DestinationCountry == destinationCountry);

        if (price.HasValue)
            allBookings = allBookings.Where(b => b.Flight.Price == price.Value);

        if (departureDate.HasValue)
            allBookings = allBookings.Where(b => b.Flight.DepartureDate.Date == departureDate.Value.Date);

        if (!string.IsNullOrEmpty(departureAirport))
            allBookings = allBookings.Where(b => b.Flight.DepartureAirport == departureAirport);

        if (!string.IsNullOrEmpty(arrivalAirport))
            allBookings = allBookings.Where(b => b.Flight.ArrivalAirport == arrivalAirport);

        if (!string.IsNullOrEmpty(passengerName))
            allBookings = allBookings.Where(b =>
                b.Passenger.FullName.Contains(passengerName, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(bookingClass))
            allBookings = allBookings.Where(b => b.Class == bookingClass);

        return allBookings.ToList();
    }
}