using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Airport_Tickets_System.UI;

public class PassengerPage
{
    private readonly Repository _repository = new Repository();
    private string _passengerUserName;

    private readonly Dictionary<string, decimal> _classPriceMultipliers =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "Economy", 1m },
            { "Business", 1.5m },
            { "First", 2m }
        };


    public void Start(string passengerUserName)
    {
        _passengerUserName = passengerUserName;
        while (true)
        {
            Console.WriteLine("\n=== Passenger Page ===");
            Console.WriteLine("1. Search for Available Flights");
            Console.WriteLine("2. Book a Flight");
            Console.WriteLine("3. Manage My Bookings");
            Console.WriteLine("4. Exit");

            var choiceInput = Console.ReadLine();
            if (!int.TryParse(choiceInput, out var choice))
            {
                Console.WriteLine("Invalid input, please enter a number between 1 and 4.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    SearchAvailableFlights();
                    break;
                case 2:
                    BookAFlight();
                    break;
                case 3:
                    ManageBookings();
                    break;
                case 4:
                    Console.WriteLine("Exiting Passenger Page...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }

    private void SearchAvailableFlights()
    {
        Console.WriteLine("\n=== Search Available Flights ===");

        var searchParams = ReadFlightSearchParameters();

        var flights = _repository.SearchFlights(
            searchParams.DepartureCountry,
            searchParams.DestinationCountry,
            searchParams.Price,
            searchParams.DepartureDate,
            searchParams.DepartureAirport,
            searchParams.ArrivalAirport,
            searchParams.Class
        );

        if (flights.Count == 0)
        {
            Console.WriteLine("No flights found with the given parameters.");
            return;
        }

        Console.WriteLine("\nAvailable Flights:");
        Console.WriteLine("Id | Departure -> Destination | DepartureDate | Airports | Classes | Base Price");
        Console.WriteLine(new string('-', 80));
        foreach (var f in flights)
        {
            Console.WriteLine($"{f.Id,-2} | {f.DepartureCountry} -> {f.DestinationCountry,-15} | " +
                              $"{f.DepartureDate:yyyy-MM-dd} | {f.DepartureAirport} -> {f.ArrivalAirport} | " +
                              $"{f.Classes,-20} | {f.Price:C}");
        }
    }

    private (string? DepartureCountry, string? DestinationCountry, decimal? Price, DateTime? DepartureDate,
        string? DepartureAirport, string? ArrivalAirport, string? Class) ReadFlightSearchParameters()
    {
        Console.Write("Departure Country (blank for any): ");
        var departureCountry = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(departureCountry))
            departureCountry = null;

        Console.Write("Destination Country (blank for any): ");
        var destinationCountry = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(destinationCountry))
            destinationCountry = null;

        Console.Write("Max Price (blank for no limit): ");
        var priceInput = Console.ReadLine();
        decimal? price = null;
        if (!string.IsNullOrWhiteSpace(priceInput) && decimal.TryParse(priceInput, out var p))
            price = p;

        Console.Write("Departure Date (yyyy-MM-dd) (blank for any): ");
        var dateInput = Console.ReadLine();
        DateTime? departureDate = null;
        if (!string.IsNullOrWhiteSpace(dateInput) && DateTime.TryParse(dateInput, out var dt))
            departureDate = dt;

        Console.Write("Departure Airport (blank for any): ");
        var departureAirport = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(departureAirport))
            departureAirport = null;

        Console.Write("Arrival Airport (blank for any): ");
        var arrivalAirport = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(arrivalAirport))
            arrivalAirport = null;

        Console.Write("Class (Economy, Business, First) (blank for any): ");
        var flightClass = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(flightClass))
            flightClass = null;

        return (departureCountry, destinationCountry, price, departureDate, departureAirport, arrivalAirport,
            flightClass);
    }

    private void BookAFlight()
    {
        Console.WriteLine("\n=== Book a Flight ===");
        var searchParams = ReadFlightSearchParameters();
        var flights = _repository.SearchFlights(
            searchParams.DepartureCountry,
            searchParams.DestinationCountry,
            searchParams.Price,
            searchParams.DepartureDate,
            searchParams.DepartureAirport,
            searchParams.ArrivalAirport,
            null);

        if (flights.Count == 0)
        {
            Console.WriteLine("No flights available for booking with these criteria.");
            return;
        }

        Console.WriteLine("\nFlights:");
        foreach (var f in flights)
        {
            Console.WriteLine(
                $"{f.Id} | {f.DepartureCountry} -> {f.DestinationCountry} | {f.DepartureDate:yyyy-MM-dd} | Price: {f.Price:C} | Classes: {f.Classes}");
        }

        Console.Write("Enter Flight Id to book: ");
        var flightIdInput = Console.ReadLine();
        if (!int.TryParse(flightIdInput, out var flightId) || flights.All(f => f.Id != flightId))
        {
            Console.WriteLine("Invalid Flight Id.");
            return;
        }

        var flightToBook = flights.First(f => f.Id == flightId);

        var availableClasses =
            flightToBook.Classes.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        Console.WriteLine("Available classes:");
        for (var i = 0; i < availableClasses.Length; i++)
        {
            var cls = availableClasses[i];
            var basePrice = flightToBook.Price;
            var multiplier = _classPriceMultipliers.GetValueOrDefault(cls, 1m);
            var priceForClass = basePrice * multiplier;
            Console.WriteLine($"{i + 1}. {cls} - Price: {priceForClass:C}");
        }

        Console.Write("Select class number: ");
        var classChoiceInput = Console.ReadLine();
        if (!int.TryParse(classChoiceInput, out var classChoice) || classChoice < 1 ||
            classChoice > availableClasses.Length)
        {
            Console.WriteLine("Invalid class choice.");
            return;
        }

        var selectedClass = availableClasses[classChoice - 1];

        Console.WriteLine($"Booking Flight {flightToBook.Id} in {selectedClass} class.");

        var bookingSuccess = _repository.BookFlight(_passengerUserName, flightToBook.Id, selectedClass);
        Console.WriteLine(bookingSuccess ? "Booking successful!" : "Booking failed.");
    }

    private void ManageBookings()
    {
        while (true)
        {
            Console.WriteLine("\n=== Manage My Bookings ===");
            Console.WriteLine("1. View My Bookings");
            Console.WriteLine("2. Cancel a Booking");
            Console.WriteLine("3. Modify a Booking");
            Console.WriteLine("4. Return to Passenger Menu");

            var input = Console.ReadLine();
            if (!int.TryParse(input, out var choice))
            {
                Console.WriteLine("Invalid input.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    ViewMyBookings();
                    break;
                case 2:
                    CancelBooking();
                    break;
                case 3:
                    ModifyBooking();
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    private void ViewMyBookings()
    {
        var bookings = _repository.GetBookingsByPassengerUserName(_passengerUserName);
        if (bookings.Count == 0)
        {
            Console.WriteLine("You have no bookings.");
            return;
        }

        Console.WriteLine("\nYour Bookings:");
        Console.WriteLine("BookingId | Flight Route               | Departure Date | Class   | Price");
        Console.WriteLine(new string('-', 75));
        foreach (var b in bookings)
        {
            Console.WriteLine(
                $"{b.BookingId,-9} | {b.Flight.DepartureCountry} -> {b.Flight.DestinationCountry,-23} | " +
                $"{b.Flight.DepartureDate:yyyy-MM-dd} | {b.Class,-7} | {b.Flight.Price,7:C}");
        }
    }

    private void CancelBooking()
    {
        Console.Write("Enter Booking ID to cancel: ");
        var input = Console.ReadLine();
        if (!int.TryParse(input, out var bookingId))
        {
            Console.WriteLine("Invalid Booking ID.");
            return;
        }

        var success = _repository.CancelBooking(bookingId, _passengerUserName);
        Console.WriteLine(success ? "Booking cancelled successfully." : "Cancellation failed or booking not found.");
    }

    private void ModifyBooking()
    {
        Console.Write("Enter Booking ID to modify: ");
        var input = Console.ReadLine();
        if (!int.TryParse(input, out var bookingId))
        {
            Console.WriteLine("Invalid Booking ID.");
            return;
        }

        var currentBooking = _repository.GetBookingById(bookingId);
        if (currentBooking.Passenger?.Username != _passengerUserName)
        {
            Console.WriteLine("Booking not found or you do not own this booking.");
            return;
        }

        Console.WriteLine("Search for a new flight for modification:");

        var searchParams = ReadFlightSearchParameters();
        var flights = _repository.SearchFlights(
            searchParams.DepartureCountry,
            searchParams.DestinationCountry,
            searchParams.Price,
            searchParams.DepartureDate,
            searchParams.DepartureAirport,
            searchParams.ArrivalAirport,
            null);

        if (flights.Count == 0)
        {
            Console.WriteLine("No flights found for modification.");
            return;
        }

        Console.WriteLine("\nAvailable Flights:");
        foreach (var f in flights)
        {
            Console.WriteLine(
                $"{f.Id} | {f.DepartureCountry} -> {f.DestinationCountry} | {f.DepartureDate:yyyy-MM-dd} | Price: {f.Price:C} | Classes: {f.Classes}");
        }

        Console.Write("Enter new Flight Id: ");
        var flightIdInput = Console.ReadLine();
        if (!int.TryParse(flightIdInput, out var newFlightId) || flights.All(f => f.Id != newFlightId))
        {
            Console.WriteLine("Invalid Flight Id.");
            return;
        }

        var flightToBook = flights.First(f => f.Id == newFlightId);

        var availableClasses =
            flightToBook.Classes.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        Console.WriteLine("Available classes:");
        for (var i = 0; i < availableClasses.Length; i++)
        {
            var cls = availableClasses[i];
            var basePrice = flightToBook.Price;
            var multiplier = _classPriceMultipliers.GetValueOrDefault(cls, 1m);
            var priceForClass = basePrice * multiplier;
            Console.WriteLine($"{i + 1}. {cls} - Price: {priceForClass:C}");
        }

        Console.Write("Select new class number: ");
        var classChoiceInput = Console.ReadLine();
        if (!int.TryParse(classChoiceInput, out var classChoice) || classChoice < 1 ||
            classChoice > availableClasses.Length)
        {
            Console.WriteLine("Invalid class choice.");
            return;
        }

        var selectedClass = availableClasses[classChoice - 1];

        var modifySuccess = _repository.ModifyBooking(bookingId, _passengerUserName, newFlightId, selectedClass);
        Console.WriteLine(modifySuccess ? "Booking modified successfully." : "Modification failed.");
    }
}