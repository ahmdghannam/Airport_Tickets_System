using System.Globalization;
using System.Text;
using Airport_Tickets_System.Utils;
using Microsoft.VisualBasic;

namespace Airport_Tickets_System.UI;

public class AdminPage
{
    private readonly Repository _repository = new Repository();
    private readonly ValidateUserInputOfFlightsUseCase _validator = new();

    public void Start()
    {
        var result = GetUserChoiceOrNull();
        switch (result)
        {
            case 1:
                ShowingAllBookings();
                break;
            case 2:
                FilterBookings();
                break;
            case 3:
                ImportFlightData();
                break;
            default:
                Console.WriteLine("Invalid input");
                break;
        }
    }

    private void FilterBookings()
    {
        Console.WriteLine("=== Filter Bookings ===");

        Console.Write("Enter Departure Country (leave blank for no filter): ");
        string departureCountry = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(departureCountry))
            departureCountry = null;

        Console.Write("Enter Destination Country (leave blank for no filter): ");
        string destinationCountry = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(destinationCountry))
            destinationCountry = null;

        Console.Write("Enter Price (leave blank for no filter): ");
        string priceInput = Console.ReadLine();
        decimal? price = null;
        if (!string.IsNullOrWhiteSpace(priceInput) && decimal.TryParse(priceInput, out decimal parsedPrice))
            price = parsedPrice;

        Console.Write("Enter Departure Date (yyyy-MM-dd) (leave blank for no filter): ");
        string dateInput = Console.ReadLine();
        DateTime? departureDate = null;
        if (!string.IsNullOrWhiteSpace(dateInput) && DateTime.TryParse(dateInput, out DateTime parsedDate))
            departureDate = parsedDate;

        Console.Write("Enter Departure Airport (leave blank for no filter): ");
        string departureAirport = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(departureAirport))
            departureAirport = null;

        Console.Write("Enter Arrival Airport (leave blank for no filter): ");
        string arrivalAirport = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(arrivalAirport))
            arrivalAirport = null;

        Console.Write("Enter Passenger Name (leave blank for no filter): ");
        string passengerName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(passengerName))
            passengerName = null;

        Console.Write("Enter Booking Class (leave blank for no filter): ");
        string bookingClass = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(bookingClass))
            bookingClass = null;

        var filteredBookings = _repository.FilterBookings(
            departureCountry,
            destinationCountry,
            price,
            departureDate,
            departureAirport,
            arrivalAirport,
            passengerName,
            bookingClass
        );

        Console.WriteLine("\n=== Filtered Bookings ===");
        foreach (var booking in filteredBookings)
        {
            Console.WriteLine(booking);
        }
    }
    
    private void ShowingAllBookings()
    {
        Console.WriteLine("=== All Bookings ===");

        // Print header
        Console.WriteLine("BookingId | Passenger Name       | Route                        | Class   | Price");
        Console.WriteLine(new string('-', 80));  // separator line

        var bookings = _repository.GetAllBookings();

        Console.WriteLine();
        foreach (var b in bookings)
        {
            Console.WriteLine($"{b.BookingId,-9} | {b.Passenger.FullName,-20} | " +
                              $"{b.Flight.DepartureCountry} -> {b.Flight.DestinationCountry,-24} | " +
                              $"{b.Class,-7} | {b.Flight.Price,7:C}");
        }
    }

    private int? GetUserChoiceOrNull()
    {
        Console.WriteLine("You entered the Admin Page !!");
        Console.WriteLine("Please Choose one of the following functionality");
        Console.WriteLine("1. show all bookings");
        Console.WriteLine("2. show bookings with filter parameters");
        Console.WriteLine("3. import flight data to the system");
        var choose = Console.ReadLine();
        var success = int.TryParse(choose, out var result);
        return success ? result : null;
    }

    void ImportFlightData()
    {
        Console.WriteLine(
            "-- Please enter the absolute path of the csv file:\n" +
            "-- note that the file should be in this format:\n" +
            "-- price, departureCountry, destinationCountry, DepartureDate, DepartureAirport, ArrivalAirport, Classes\n" +
            "-- IMPORTANT: first line will be skipped as we assume it is a header line"
        );
        var absolutePath = Console.ReadLine();
        if (string.IsNullOrEmpty(absolutePath) || !File.Exists(absolutePath))
        {
            Console.WriteLine("File Path Not Valid !");
            return;
        }

        var lines = File.ReadAllLines(absolutePath);
        if (_validator.ValidateFlightsInputData(lines))
        {
            var successFlag = _repository.InsertFlightsData(lines);
            var message = successFlag ? "Flights Data Inserted Successfully." : "Insertion Failed!";
            Console.WriteLine(message);
        }
    }
}